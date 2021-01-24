using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        private GetCart GetCart { get; set; }
        public CartViewComponent([FromServices] GetCart getCart)
        {
            GetCart = getCart;
        }

        public async Task<IViewComponentResult> InvokeAsync(string componentName)
        {
            var items = await GetCart.DoAsync();
            return View(componentName, items.ToList());
        }
    }
}
