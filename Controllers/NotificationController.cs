using LionTaskManagementApp.Areas.Identity.Data;
using LionTaskManagementApp.Data;
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
            var notifications = await _context.Notifications.Where(u => u.RecipientId.Equals(user.Id)).ToListAsync();
            return View(notifications);
        }
    }
}
