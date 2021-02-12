using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shop.Application.Products;
using Shop.Database;

namespace Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<GetProducts.Response> Products { get; set; }

        public async Task OnGetAsync([FromServices] GetProducts getProducts)
        {
            Products = await getProducts.DoAsync();
        }
        
    }
}
