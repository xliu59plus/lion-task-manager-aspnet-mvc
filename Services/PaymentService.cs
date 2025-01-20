using LionTaskManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace LionTaskManagementApp.Services
{
    public class PaymentService
    {
        private readonly S3Service _s3Service;
        private readonly String _domain;

        public PaymentService(S3Service s3Service)
        {
            _s3Service = s3Service;
            _domain = Environment.GetEnvironmentVariable("TaskManagerDomain") ?? "https://localhost:7227";
        }

        public async Task<Session> CreatePayment(TaskModel taskModel, decimal cost) {
            List<ProductEntity> productList = new List<ProductEntity>();
            
            // 1. create product,
            var productOption = new ProductCreateOptions
            {
                Name = taskModel.Title,
                Description = taskModel.Description,
                Images = new List<string> {await _s3Service.GetPreSignedUrlAsync(taskModel.ArtworkKey, TimeSpan.FromMinutes(10))}
            };

            var productService = new ProductService();
            var product = productService.Create(productOption);

            // 2. create price
            var priceOptions = new PriceCreateOptions
            {
                Product = product.Id,
                UnitAmount = (long?)cost,
                Currency = "usd",
            };

            var priceService = new PriceService();
            var priceItem = priceService.Create(priceOptions);

            // 3. create the payment session
            var checkoutOption = new SessionCreateOptions
            {
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions> {
                    new SessionLineItemOptions {
                        Price = priceItem.Id,
                        Quantity = 1,
                    }
                },
                SuccessUrl = _domain + "/Checkout/PaymentSuccessful?sessionId={CHECKOUT_SESSION_ID}&taskId=" + taskModel.Id,
                CancelUrl = _domain + "/Checkout/PaymentFailed?sessionId={CHECKOUT_SESSION_ID}&taskId=" + taskModel.Id,
            };

            var sessionService = new SessionService();
            Session session = sessionService.Create(checkoutOption);

            return session;
        }

        public async Task<Session> GetPaymentDetailsAsync(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);
            return session;
        }

    }
}
