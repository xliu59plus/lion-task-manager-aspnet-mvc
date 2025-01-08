using Stripe;
using Stripe.Checkout;

namespace LionTaskManagementApp.Services
{
    public class PaymentService
    {
        public PaymentService()
        {
            StripeConfiguration.ApiKey = "your_stripe_secret_key";
        }

        public async Task<Session> CreateCheckoutSessionAsync(List<ProductEntity> products, string successUrl, string cancelUrl, string customerEmail)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = customerEmail
            };

            foreach (var item in products)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * item.Quantity),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),
                        }
                    },
                    Quantity = item.Quantity
                };

                options.LineItems.Add(sessionListItem);
            }

            var service = new SessionService();
            return await service.CreateAsync(options);
        }

        public async Task<Account> CreateConnectedAccountAsync(string email, string bankAccountNumber, string bankRoutingNumber)
        {
            var options = new AccountCreateOptions
            {
                Type = "custom",
                Country = "US",
                Email = email,
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                    Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
                },
                ExternalAccount = new AccountBankAccountOptions
                {
                    AccountNumber = bankAccountNumber,
                    RoutingNumber = bankRoutingNumber,
                    Country = "US",
                    Currency = "usd",
                }
            };
            var service = new AccountService();
            return await service.CreateAsync(options);
        }

        public async Task<Payout> CreatePayoutAsync(long amount, string currency, string stripeAccountId)
        {
            var options = new PayoutCreateOptions
            {
                Amount = amount,
                Currency = currency,
            };
            var service = new PayoutService();
            return await service.CreateAsync(options, new RequestOptions { StripeAccount = stripeAccountId });
        }

        public async Task<Refund> CreateRefundAsync(string paymentIntentId)
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
            };
            var service = new RefundService();
            return await service.CreateAsync(options);
        }
    }
}
