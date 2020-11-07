using Microsoft.AspNetCore.Mvc;
using Shop.Application;
using Shop.Application.ViewModels;
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
        public IActionResult CreateProduct([FromBody]ProductViewModel vm) => Ok(new CreateProduct(_context).Do(vm));

        [HttpDelete("products/{id}")]
        public IActionResult DeleteProduct(int id) => Ok(new DeleteProduct(_context).Do(id));
    }
}
