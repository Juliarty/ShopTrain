using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Products
{
    public class GetStocks
    {
        private ApplicationDbContext _context;

        public GetStocks(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Response> Do(int productId) {

            var product = _context.Products
            .Include(x => x.Stock)
            .FirstOrDefault(x => x.Id == productId);

            if (product == null) return Enumerable.Empty<Response>();

            var stock = product.Stock;

            return stock.Select(y => new Response
            {
                Id = y.Id,
                Name = y.Description,
                Qty = y.Qty
            }).ToList();

        } 

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Qty { get; set; }
        }
    }
}
