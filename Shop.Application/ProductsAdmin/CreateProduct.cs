using Shop.Database;
using Shop.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
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

        public async Task<Response> Do(Request productViewModel)
        {         
            var product = new Product()
            {
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Value = Utils.GetDecimal(productViewModel.Value)
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value.ToString()
            };
        }

        public class Request
        {
            [Required]
            [StringLength(30)]
            public string Name { get; set; }
            [Required]
            [StringLength(130)]
            public string Description { get; set; }

            [RegularExpression(@"\d+([,\.]\d+)?", ErrorMessage = "This price fromat is not supported.")]
            [Required]
            [StringLength(30)]
            public string Value { get; set; }

        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }

        }

    }

 
}
