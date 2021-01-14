using Microsoft.AspNetCore.Mvc;
using Shop.Application.Products;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    // ToDo: add policy to customer
    // [Authorize(Policy = "Manager")]
    public class ProductController: Controller
    {
        private readonly ApplicationDbContext _ctx;

        public ProductController(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("stocks")]
        public int GeStocks(int productId, int stockId)
        {

            var stocks = new GetStocks(_ctx).Do(productId).ToList();

            var stock = stocks.FirstOrDefault(x => x.Id == stockId);

            if (stock == null) return 0;

            return stock.Qty;
        }


    }
}
