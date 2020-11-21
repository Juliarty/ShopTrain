using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Products
{
    public class GetProducts
    {
        private ApplicationDbContext _context;

        public GetProducts(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Response> Do() =>
            _context.Products.ToList().Select(x => new Response
            {
                Id = x.Id,
                Name = x.Name,
                Value = $"${x.Value:N2}"
            });

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
