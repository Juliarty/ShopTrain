using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Shop.Database;
using Stripe;
using Stripe.Checkout;

namespace Shop.UI.Checkout
{
    public class PaymentModel : PageModel
    {
        public string PublicKey { get; }

        private ApplicationDbContext _ctx;

        public PaymentModel(IConfiguration config, ApplicationDbContext context)
        {
            PublicKey = config["Stripe:PublicKey"].ToString();
            _ctx = context;
        }


        public IActionResult OnGet()
        {

            var information = new GetCustomerInformation(HttpContext.Session).Do();
            if (information == null)
            {
                return RedirectToPage("/Checkout/CustomerInformation");
            }

            var cartOrder = new Application.Cart.GetOrder(HttpContext.Session, _ctx).Do();


            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var cartOrder = new Application.Cart.GetOrder(HttpContext.Session, _ctx).Do();

            await new CreateOrder(_ctx).Do(new CreateOrder.Request()
            {
                StripeReference = "Charge.Id",
                FirstName = cartOrder.CustomerInformation.FirstName,
                LastName = cartOrder.CustomerInformation.LastName,
                Email = cartOrder.CustomerInformation.Email,
                PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,
                Address1 = cartOrder.CustomerInformation.Address1,
                Address2 = cartOrder.CustomerInformation.Address2,
                City = cartOrder.CustomerInformation.City,
                PostCode = cartOrder.CustomerInformation.PostCode,
                Stocks = cartOrder.Products.Select(x => new CreateOrder.Stock() { StockId = x.StockId, Qty = x.Qty}).ToList()
                
            });;

            return RedirectToPage("/Index");
        }
    }
}
