using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

public class CheckOutExampleController: Controller
{
    public IActionResult Index()
    {
        List<ProductEntity> productList = new List<ProductEntity>();
        productList = new List<ProductEntity>{
            new ProductEntity {
                Product = "TH",
                Rate = 1500,
                Quantity=2,
                //ImagePath="aa"
            },
            new ProductEntity {
                Product = "TimeWear",
                Rate = 1500,
                Quantity=1,
                // ImagePath="aa"
            },
        };

        return View(productList);
    }

    public IActionResult Create()
    {
        try {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>{
                new ProductEntity {
                    Product = "TH",
                    Rate = 1500,
                    Quantity=2,
                    //ImagePath="aa"
                },
                new ProductEntity {
                    Product = "TimeWear",
                    Rate = 1500,
                    Quantity=1,
                    // ImagePath="aa"
                },
            };

        var domain="https://localhost:7227";

            var options = new SessionCreateOptions 
            {
                SuccessUrl=domain + "/Checkout/OrderConfirmation",
                CancelUrl = domain + "/Checkout/OrderConfirmation",
                LineItems = new List<SessionLineItemOptions>(),
                Mode="payment",
                CustomerEmail="anyemail@gmail.com"
            };

            foreach(var item in productList) {
                var sessionListItem = new SessionLineItemOptions {
                    PriceData=new SessionLineItemPriceDataOptions {
                        UnitAmount=(long)(item.Rate*item.Quantity),
                        Currency="cad",
                        ProductData=new SessionLineItemPriceDataProductDataOptions {
                            Name=item.Product.ToString(),
                        }
                    },
                    Quantity=item.Quantity
                };

                options.LineItems.Add(sessionListItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
            return new StatusCodeResult(303);
        }
    }

    public IActionResult OrderConfirmation() 
    {
        var service = new SessionService();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var sessionId = TempData["Session"] == null? "": TempData["Session"].ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        Session session = service.Get(sessionId);
        ViewBag.OrderID = "sample order ID";
        ViewBag.Amount = 1000;
        ViewBag.PaymentMethod = "Sample Payment method";
        if(session.PaymentStatus == "Paid") {
            return View("payment-succeed");
        } else {
            return View("payment-failed");
        }

    }
}