using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Shop.Application.Cart;
using Shop.Domain.Infrastructure;
using Shop.Application.Orders;
using Shop.Database;
using Stripe;
using Stripe.Checkout;

namespace Shop.UI.Checkout
{
    public class PaymentModel : PageModel
    {
        public string PublicKey { get; }

        private readonly IOrderManager _orderManager;
        private readonly IStockManager _stockManager;
        private readonly ISessionManager _sessionManager;

        [BindProperty]
        public Application.Cart.GetOrder.Response CartOrder { get; set; }
        public PaymentModel(IConfiguration config, IOrderManager orderManager, ISessionManager sessionManager, IStockManager stockManager)
        {
            PublicKey = config["Stripe:PublicKey"].ToString();
            _orderManager = orderManager;
            _sessionManager = sessionManager;
            _stockManager = stockManager;
        }


        public IActionResult OnGet()
        {

            var information = new GetCustomerInformation(_sessionManager).Do();
            if (information == null)
            {
                return RedirectToPage("/Checkout/CustomerInformation");
            }

            CartOrder = new Application.Cart.GetOrder(_sessionManager).Do();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var cartOrder = new Application.Cart.GetOrder(_sessionManager).Do();


            await new CreateOrder(_orderManager, _stockManager).Do(new CreateOrder.Request()
            {
                //ToDo: change stripeReference
                StripeReference = "Charge.Id",
                SessionId = HttpContext.Session.Id,
                FirstName = cartOrder.CustomerInformation.FirstName,
                LastName = cartOrder.CustomerInformation.LastName,
                Email = cartOrder.CustomerInformation.Email,
                PhoneNumber = cartOrder.CustomerInformation.PhoneNumber,
                Address1 = cartOrder.CustomerInformation.Address1,
                Address2 = cartOrder.CustomerInformation.Address2,
                City = cartOrder.CustomerInformation.City,
                PostCode = cartOrder.CustomerInformation.PostCode,
                Stocks = cartOrder.Products.Select(x => new CreateOrder.Stock() { StockId = x.StockId, Qty = x.Qty}).ToList()
                
            });

            _sessionManager.ClearCart();
            return RedirectToPage("/Index");
        }
    }
}
