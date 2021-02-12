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

        public IViewComponentResult Invoke(string componentName)
        {
            return View(componentName, GetCart.Do());
        }
    }
}
