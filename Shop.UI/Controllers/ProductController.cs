using Microsoft.AspNetCore.Mvc;
using Shop.Application.Products;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    // ToDo: add policy to customer
    // [Authorize(Policy = "Manager")]
    public class ProductController: Controller
    {
        /// <summary>
        /// Returns the number of products available in stock.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stockId"></param>
        /// <returns></returns>
        [HttpGet("stocks")]
        public async Task<int> GeStocks(int productId, int stockId, [FromServices] GetProduct getProduct)
        {

            var product = await getProduct.DoAsync(productId);

            var stock = product.Stock.FirstOrDefault(x => x.Id == stockId);

            if (stock == null) return 0;

            return stock.Qty;
        }


    }
}
