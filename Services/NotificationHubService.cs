using LionTaskManagementApp.Areas.Identity.Data;
using LionTaskManagementApp.Data;
using LionTaskManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LionTaskManagementApp.Services.Hubs
{
    public class NotificationHubService: Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHubService> _hubContext;
        public NotificationHubService(IHubContext<NotificationHubService> hubContext, ApplicationDbContext context)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SendMessage(string user, string message)
        {
            // await Clients.All.SendAsync("NotificationMessage", user, message);
            await _hubContext.Clients.User(user).SendAsync("NotificationMessage", message);
        }
    }
}
