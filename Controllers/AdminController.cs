using LionTaskManagementApp.Areas.Identity.Data;
using LionTaskManagementApp.Data;
using LionTaskManagementApp.Services.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LionTaskManagementApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TaskUser> _userManager;
        private readonly NotificationHubService _notificationHubService;

        public AdminController(ApplicationDbContext context, UserManager<TaskUser> userManager, NotificationHubService notificationHubService)
        {
            _context = context;
            _userManager = userManager;
            _notificationHubService = notificationHubService;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.ActivationRequests
                .OrderBy(r => r.IsApproved)
                .ThenBy(r => r.LastUpdateTime)
                .ToListAsync();
            return View(requests);
        }

        public async Task<IActionResult> ViewActivationRequestDetail(string userId, string requestRole)
        {
            var request = await _context.ActivationRequests
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RequestedRole == requestRole);
            if (request == null)
            {
                return NotFound();
            }

            if (requestRole == "Contractor")
            {
                var contractorInfo = await _context.ContractorInfos.FirstOrDefaultAsync(c => c.UserId == userId);
                if (contractorInfo == null)
                {
                    return NotFound();
                }
                return View("ContractorActivationRequestDetail", contractorInfo);
            }
            else if (requestRole == "Poster")
            {
                var posterInfo = await _context.PosterInfos.FirstOrDefaultAsync(p => p.PosterId == userId);
                if (posterInfo == null)
                {
                    return NotFound();
                }
                return View("PosterActivationRequestDetail", posterInfo);
            }

            return BadRequest("Invalid role specified.");
        }



        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var request = await _context.ActivationRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.IsApproved = true;
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, request.RequestedRole);
                await _userManager.RemoveFromRoleAsync(user, "Inactive_" + request.RequestedRole);
                await _notificationHubService.SendMessage(user.Id, "Your profile is activated now, please logout and login again.");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DenyRequest(int id, string denyComments)
        {
            var request = await _context.ActivationRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.IsApproved = false;
            request.DenyComents = denyComments;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
