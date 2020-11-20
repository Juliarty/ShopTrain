using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Application;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ApplicationDbContext _context;

        [BindProperty]
        public CreateProduct.Request Product { get; set; }
        public IEnumerable<GetProducts.Response> Products { get; set; }
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            Products = new GetProducts(_context).Do();
        }

              
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("Index");
            }

            await new CreateProduct(_context).Do(Product);
            return RedirectToPage("Index");
        }
      
    }
}
