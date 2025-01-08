using LionTaskManagementApp.Areas.Identity.Data;
using LionTaskManagementApp.Data;
using LionTaskManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LionTaskManagementApp.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TaskUser> _userManager;

        public NotificationController(ApplicationDbContext context, UserManager<TaskUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            // Retrieve all notifications from the database
            var notifications = await _context.TaskNotifications.Where(u => u.RecipientId.Equals(user.Id)).ToListAsync();

            List<ContractorNotificationModel> contractorNotificationList = new List<ContractorNotificationModel>();
            foreach (var notification in notifications) {
                var task = await _context.Tasks.FindAsync(notification.TaskId);
                if(task == null) {  continue; }

                var contractorNotification = new ContractorNotificationModel
                {
                    IsNotified = true,
                    IsRead = false,
                    Budget = task.Budget,
                    TaskId = task.Id,
                    Title = task.Title,
                    Distance = notification.Distance,
                    NotifiedUserId = notification.RecipientId,
                    CreatedDatetime = notification.MessageTimestamp,
                    LastUpdatedDatetime = task.LastUpdatedTime,
                    ExpirationDatetime = task.Deadline
                };

                contractorNotificationList.Add(contractorNotification);
            }

            return View(contractorNotificationList);
        }
    }
}
