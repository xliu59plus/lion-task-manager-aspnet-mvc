using LionTaskManagementApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Models;
using LionTaskManagementApp.Models.Poster;

namespace LionTaskManagementApp.Data;

public class ApplicationDbContext : IdentityDbContext<TaskUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskModel> Tasks { get; set; } = default!;
    public DbSet<ContractorInfo> ContractorInfos { get; set; }
    public DbSet<PosterInfo> PosterInfos { get; set; }
    public DbSet<TaskNotification> TaskNotifications { get; set; }
    public DbSet<CreateTaskNotificationQueueModel> CreateTaskNotificationQueueModels { get; set; }
}
