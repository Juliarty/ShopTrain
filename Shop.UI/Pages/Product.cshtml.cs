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

        [BindProperty]
        public GetProduct.Response Product { get; set; }

        public async Task OnGetAsync(string name)
        {
            Product = await new GetProduct(_ctx).DoAsync(name.Replace("-", " "));
        }
        public async Task<IActionResult> OnPostAsync([FromServices] AddToCart addToCart)
        {
            //var currentId = HttpContext.Session.GetString("id");
            //HttpContext.Session.SetString("id", "");

            //ToDo: check whether any stock was selected
            var stockAddedToCart = await addToCart.DoAsync(CartViewModel);
            if (stockAddedToCart)
                return RedirectToPage("Cart");
            else
                //ToDo: Redirect to warning
                return Page();
        }

        //public ContentResult OnGetStockQty(int stockId)
        //{
        //    var stocks = new GetStocks(_ctx).Do(Product.Id);
        //    var currentStock = stocks.FirstOrDefault(x => x.Id == stockId);
        //    if(currentStock == null) throw new Exception($"There is no stock for product {Product.Name} with id: {stockId}");
        //    if(currentStock.Qty == 0)
        //    {

        //    }
        //    return Content(currentStock.Qty.ToString());
        //}
    }
}
