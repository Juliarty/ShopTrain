using Microsoft.AspNetCore.Mvc;
using Shop.Application.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]/[action]")] 
    public class CartController : Controller
    {

        [Route("/[controller]/items")]
        [HttpGet("")]
        public IEnumerable<GetCart.Response> GetCart([FromServices] GetCart getCart)
        {
          return getCart.Do();
        }

        [HttpGet("")]
        public IActionResult GetSmallCart()
        {
            return ViewComponent("Cart", new { componentName = "Small" });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddOneAsync(int id, [FromServices] AddToCart addToCart)
        {

            var success = await addToCart.DoAsync(new AddToCart.Request()
            {
                StockId = id,
                Qty = 1,
            });


            if (success)
            {
                return Ok("Item was added successfully.");
            }

            return BadRequest("Failed to add to the cart.");
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> SubOne(int stockId, [FromServices] RemoveFromCart removeFromCart)
        {

            var success = await removeFromCart.DoAsync(new RemoveFromCart.Request()
            {
                StockId = stockId,
                Qty = 1,
                RemoveAll = false
            });

            if (success) 
            {
                return Ok("Item was subtructed successfully");      
            }

            return BadRequest("Failed to subtract from the cart");
        }


        [HttpPost("{stockId}")]
        public async Task<IActionResult> RemoveItem(int stockId, [FromServices] RemoveFromCart removeFromCart)
        {
            var success = await removeFromCart.DoAsync(new RemoveFromCart.Request()
            {
                StockId = stockId,
                Qty = 0,
                RemoveAll = true
            });

            if (success)
            {
                return Ok("Item was removed successfully");
            }

            return BadRequest("Failed to remove from the cart");

        }

    }
}
