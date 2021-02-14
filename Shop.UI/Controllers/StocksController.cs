using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.StocksAdmin;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class StocksController : Controller
    {
        [HttpGet("")]
        public IActionResult GetStock([FromServices] GetStock getStock) => Ok(getStock.Do());

        [HttpPost("")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStock.Request vm,
            [FromServices] CreateStock createStock) => Ok(await createStock.Do(vm));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id, [FromServices] DeleteStock deleteStock) => 
            Ok(await deleteStock.Do(id));

        [HttpPut("")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStock.Request request, 
            [FromServices] UpdateStock updateStock) => Ok(await updateStock.Do(request));
    }
}
