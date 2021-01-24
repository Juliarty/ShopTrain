using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Infrastructure;
using Shop.Database;

namespace Shop.UI.Checkout
{
    public class CustomerInformationModel : PageModel
    {
        private IWebHostEnvironment _env;
        private ApplicationDbContext _ctx;
        private ISessionManager _sessionManager;

        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation { get; set; }
        public CustomerInformationModel(IWebHostEnvironment env, ApplicationDbContext ctx, ISessionManager sessionManager)
        {
            _env = env;
            _ctx = ctx;
            _sessionManager = sessionManager;
        }
        public IActionResult OnGet()
        {
            // get customer info

            var information = new GetCustomerInformation(_sessionManager).Do();
            if (information == null)
            {
                if (_env.EnvironmentName == "Development")
                {
                    CustomerInformation = 
                        new AddCustomerInformation.Request()
                        {
                            FirstName = "Andrew",
                            LastName = "Smith",
                            Email = "mail@mail.ru",
                            PhoneNumber = "+722222222",
                            Address1 = "Somewhere",
                            Address2 = "Nowhere",
                            City = "Moscow",
                            PostCode = "123"
                        };
                }
                return Page();
            }

            // if info exist go to payment

            return RedirectToPage("/Checkout/Payment");
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //post customer info
            new AddCustomerInformation(_sessionManager).Do(CustomerInformation);
            HttpContext.Response.Cookies.Append(
                "customer-info",
                HttpContext.Session.GetString("customer-info"),
                new CookieOptions()
                {
                    MaxAge = TimeSpan.FromDays(365)
                });
            return RedirectToPage("/Checkout/Payment");
        }
    }
}
