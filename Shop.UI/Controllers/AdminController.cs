using Microsoft.AspNetCore.Mvc;
using Shop.Application;
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
        public IActionResult GetProducts() => Ok(new Application.GetProducts.GetProducts(_context).Do());
       
        [HttpGet("products/{id}")]
        public IActionResult GetProducts(int id) => Ok(new Application.GetProduct.GetProduct(_context).Do(id));

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] Application.CreateProduct.Request vm) => Ok(await new Application.CreateProduct.CreateProduct(_context).Do(vm));

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id) => Ok(await new Application.DeleteProduct.DeleteProduct(_context).Do(id));

        [HttpPut("products")]
        public async Task<IActionResult> UpdateProduct([FromBody]Application.UpdateProduct.Request request) => Ok(await new Application.UpdateProduct.UpdateProduct(_context).Do(request));
    }
}
