using Microsoft.AspNetCore.Mvc;
using Shop.Application;
using Shop.Application.StockAdmin;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    public class AdminController: Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        [HttpGet("products")]
        public IActionResult GetProducts() => Ok(new GetProducts(_context).Do());
       
        [HttpGet("products/{id}")]
        public IActionResult GetProducts(int id) => Ok(new GetProduct(_context).Do(id));

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct.Request vm) => Ok(await new CreateProduct(_context).Do(vm));

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id) => Ok(await new DeleteProduct(_context).Do(id));

        [HttpPut("products")]
        public async Task<IActionResult> UpdateProduct([FromBody]UpdateProduct.Request request) => Ok(await new UpdateProduct(_context).Do(request));




        [HttpGet("stocks")]
        public IActionResult GetStock() => Ok(new GetStock(_context).Do());

        [HttpPost("stocks")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStock.Request vm) => Ok(await new CreateStock(_context).Do(vm));

        [HttpDelete("stocks/{id}")]
        public async Task<IActionResult> DeleteStock(int id) => Ok(await new DeleteStock(_context).Do(id));

        [HttpPut("stocks")]
        public async Task<IActionResult> UpdateStock ([FromBody] UpdateStock.Request request) => Ok(await new UpdateStock(_context).Do(request));
    }
}
