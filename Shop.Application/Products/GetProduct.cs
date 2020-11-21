using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private ApplicationDbContext _context;

        public GetProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public Response Do(string name) =>
            _context.Products.Where(x =>x.Name == name)
            .Include(x => x.Stock)
            .Select(x => new Response
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Value = $"${x.Value:N2}",
                Stock = x.Stock.Select(y => new StockViewModel 
                { 
                    Id = y.Id,
                    Description = y.Description,
                    InStock = y.Qty > 0                
                }).ToList()
            }).FirstOrDefault();

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class StockViewModel 
        {
            public int Id;
            public string Description;
            public bool InStock;
        }

    }
}
