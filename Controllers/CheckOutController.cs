using LionTaskManagementApp.Data;
using LionTaskManagementApp.Models;
using LionTaskManagementApp.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LionTaskManagementApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly ApplicationDbContext _dbContext;


        public CheckoutController(PaymentService paymentService, ApplicationDbContext dbContext)
        {
            _paymentService = paymentService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> PaymentSuccessful(string sessionId, int taskId)
        {
            var taskModel = await _dbContext.Tasks.FindAsync(taskId);
            if (taskModel == null)
            {
                return NotFound();
            }

            var session = await _paymentService.GetPaymentDetailsAsync(sessionId);

            ViewBag.Message = "Payment successful. Contractor will start working.";
            ViewBag.PaymentDetails = session;

            taskModel.Status = MyTaskStatus.ReadyToStart.ToString();
            taskModel.PaymentSessionId = sessionId;
            taskModel.PaymentIntentId = session.PaymentIntentId;
            _dbContext.Tasks.Update(taskModel);
            await _dbContext.SaveChangesAsync();

            return View(taskModel);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentFailed(string sessionId, int taskId)
        {
            var taskModel = await _dbContext.Tasks.FindAsync(taskId);
            if (taskModel == null)
            {
                return NotFound();
            }

            var session = await _paymentService.GetPaymentDetailsAsync(sessionId);
            if (session.PaymentStatus != "failed")
            {
                return BadRequest("Invalid payment status.");
            }

            taskModel.Status = MyTaskStatus.Initialized.ToString();
            taskModel.TakenById = null;
            _dbContext.Tasks.Update(taskModel);
            await _dbContext.SaveChangesAsync();

            ViewBag.Message = "Payment failed.";

            return View(taskModel);
        }
    }
}
