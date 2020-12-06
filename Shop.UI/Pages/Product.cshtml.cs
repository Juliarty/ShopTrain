using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.Cart;
using Shop.Application.Products;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class ProductModel : PageModel
    {
        private ApplicationDbContext _ctx;

        public ProductModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        [BindProperty]
        public AddToCart.Request CartViewModel { get; set; }

        public GetProduct.Response Product { get; set; }

        public async Task OnGetAsync(string name)
        {
            Product = await new GetProduct(_ctx).Do(name.Replace("-", " "));
        }
        public async Task<IActionResult> OnPostAsync()
        {
            //var currentId = HttpContext.Session.GetString("id");
            //HttpContext.Session.SetString("id", "");

            //ToDo: check whether any stock was selected
            var stockAddedToCart = await new AddToCart(_ctx, HttpContext.Session).Do(CartViewModel);
            if (stockAddedToCart)
                return RedirectToPage("Cart");
            else
                //ToDo: Redirect to warning
                return Page();
        }
    }
}
