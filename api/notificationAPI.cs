using LionTaskManagementApp.Services.Hubs; // Replace with your hub's namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LionTaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessageController : ControllerBase
    {
        private readonly NotificationHubService _notificationService;


        public SendMessageController(NotificationHubService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            if (!string.IsNullOrEmpty(request.userId) && !string.IsNullOrEmpty(request.message))
            {
                await _notificationService.SendMessage(request.userId, request.message);
                return Ok(); // Indicate success
            }

            return BadRequest(); // Indicate missing data
        }
    }

    // Create a class to represent the request body
    public class SendMessageRequest
    {
        public string userId { get; set; }
        public string message { get; set; }
    }
}