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
        [BindProperty]
        public AddToCart.Request CartViewModel { get; set; }

        [BindProperty]
        public GetProduct.Response Product { get; set; }

        public async Task OnGetAsync(int productId, [FromServices] GetProduct getProduct)
        {
            Product = await getProduct.DoAsync(productId);
        }
        public async Task<IActionResult> OnPostAsync([FromServices] AddToCart addToCart)
        {
            //ToDo: check whether any stock was selected
            var stockAddedToCart = await addToCart.DoAsync(CartViewModel);
            if (stockAddedToCart)
                return RedirectToPage("Cart");
            else
                //ToDo: Redirect to warning
                return Page();
        }
    }
}
