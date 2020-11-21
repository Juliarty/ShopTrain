using Shop.Database;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin
{
    public class UpdateProduct
    {
        private ApplicationDbContext _context;

        public UpdateProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Do(Request request)
        {
            var product = _context.Products.ToList().FirstOrDefault(x => x.Id == request.Id);

            product.Name = request.Name;
            product.Description = request.Description;
            product.Value = Utils.GetDecimal(request.Value);

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
            public int Id { get; set; }
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
