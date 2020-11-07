using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Shop.Application
{
    public class CreateProduct
    {
        private ApplicationDbContext _context;

        public CreateProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Do(ViewModels.ProductViewModel productViewModel)
        {
            _context.Products.Add(new Product()
            {
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Value = productViewModel.Value
            });

            await _context.SaveChangesAsync();
        }

       
    }
}
