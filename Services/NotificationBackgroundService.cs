using LionTaskManagementApp.Data;
using LionTaskManagementApp.Models;
using LionTaskManagementApp.Services.Hubs;
using LionTaskManagementApp.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class NotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentQueue<(TaskModel, string)> _notificationQueue = new ConcurrentQueue<(TaskModel, string)>();

    public NotificationBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void QueueNotification(TaskModel taskModel, string currentUserId)
    {
        _notificationQueue.Enqueue((taskModel, currentUserId));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_notificationQueue.TryDequeue(out var notification))
            {
                await ProcessNotificationAsync(notification.Item1, notification.Item2, stoppingToken);
            }

            await Task.Delay(1000, stoppingToken); // Adjust the delay as needed
        }
    }

    private async Task ProcessNotificationAsync(TaskModel taskModel, string currentUserId, CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notificationHubService = scope.ServiceProvider.GetRequiredService<NotificationHubService>();

            try
            {
                // 1. get all available users.
                var contractors = await context.ContractorInfos.ToListAsync();
                if (contractors.Count == 0)
                {
                    return;
                }

                // 2. add all of them into createTaskNotificationModels.
                var notifyingList = new List<CreateTaskNotificationQueueModel>();
                foreach (var c in contractors)
                {
                    notifyingList.Add(new CreateTaskNotificationQueueModel
                    {
                        NotifiedUserId = c.UserId,
                        TaskId = taskModel.Id,
                        Distance = DistanceCalculator.CalculateDistance(taskModel.Latitude, taskModel.Longitude, c.Latitude, c.Longitude),
                        IsNotified = false,
                    });
                }

                context.CreateTaskNotificationQueueModels.AddRange(notifyingList);
                await context.SaveChangesAsync();

                // looping step
                while (true)
                {
                    // 3.0 check if the task is assigned.
                    var updatedTask = await context.Tasks.FindAsync(taskModel.Id);
                    if (updatedTask == null || !string.IsNullOrEmpty(updatedTask.TakenById))
                    {
                        break;
                    }

                    // 3. pick 1 from createTaskNotificationModels.
                    var notificationObjects = await context.CreateTaskNotificationQueueModels
                                                            .Where(n => n.TaskId == taskModel.Id && !n.IsNotified)
                                                            .OrderBy(n => n.Distance)
                                                            .Take(1)
                                                            .ToListAsync();
                    if (notificationObjects.Count == 0)
                    {
                        return;
                    }

                    var persistNotificationList = new List<TaskNotification>();
                    // 3.5 add to task notificationList.
                    foreach (var entry in notificationObjects)
                    {
                        await notificationHubService.SendMessage(entry.NotifiedUserId, "You have new task invite!");
                        persistNotificationList.Add(new TaskNotification
                        {
                            IsRead = false,
                            SenderId = currentUserId,
                            RecipientId = entry.NotifiedUserId,
                            Message = "You have new task invite, visit home page now!",
                            TaskId = taskModel.Id,
                            MessageTimestamp = DateTimeOffset.UtcNow,
                            Distance = entry.Distance,
                        });

                        entry.IsNotified = true;
                    }

                    context.TaskNotifications.AddRange(persistNotificationList);
                    await context.SaveChangesAsync();

                    // 5. tread stop for 5 min, and then loop again.
                    await Task.Delay(TimeSpan.FromSeconds(120), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Exception in pushNotificationAsync: {ex.Message}");
            }
        }
    }
}
