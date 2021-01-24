using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]/[action]")] 
    public class CartController : Controller
    {

        [Route("/[controller]/items")]
        [HttpGet("")]
        public async Task<List<GetCart.Response>> GetCartAsync([FromServices] GetCart getCart)
        {
            var result = await getCart.DoAsync();
            return result.ToList();
        }

        [HttpGet("")]
        public IActionResult GetSmallCart()
        {

            //return Ok(ViewComponent("Cart", new { componentName = "Small" }));
            return new ViewComponentResult
            {
                ViewComponentName = "Cart",
                Arguments = new { componentName = "Small" },
                ViewData = this.ViewData,
                TempData = this.TempData
            };
            var t = ViewComponent("Cart", new { componentName ="Small" });
            return t;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddOneAsync(int id, [FromServices] AddToCart addToCart)
        {
            var request = new AddToCart.Request()
            {
                StockId = id,
                Qty = 1,
            };

            var success = await addToCart.DoAsync(request);
            if (success)
            {
                return Ok("Item was added successfully.");
            }


            return BadRequest("Failed to add to the cart.");
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> SubOne(int stockId, [FromServices] AddToCart addToCart)
        {
            var request = new AddToCart.Request()
            {
                StockId = stockId,
                Qty = -1,
            };

            var success = await addToCart.DoAsync(request);
            if (success) 
            {
                return Ok("Item was subtructed successfully");      
            }


            return BadRequest("Failed to subtract from the cart");
        }


        [HttpPost("{stockId}")]
        public async Task<IActionResult> RemoveItem(int stockId, [FromServices] RemoveItem removeItem)
        {
            var success = await removeItem.DoAsync(stockId);
            if (success)
            {
                return Ok("Item was removed successfully");
            }

            return BadRequest("Failed to remove from the cart");

        }

    }
}
