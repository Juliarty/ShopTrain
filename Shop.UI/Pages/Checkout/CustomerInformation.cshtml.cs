using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Database;

namespace Shop.UI.Checkout
{
    public class CustomerInformationModel : PageModel
    {
        private ApplicationDbContext _ctx;

        [BindProperty]
        public AddCustomerInformation.Request CustomerInformation { get; set; }
        public CustomerInformationModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult OnGet()
        {
            // get customer info

            var information = new GetCustomerInformation(HttpContext.Session).Do();
            if (information == null)
            {
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
            new AddCustomerInformation(HttpContext.Session).Do(CustomerInformation);
            return RedirectToPage("/Checkout/Payment");
        }
    }
}
