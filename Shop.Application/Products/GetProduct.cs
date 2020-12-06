using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class GetProduct
    {
        private ApplicationDbContext _ctx;

        public GetProduct(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<Response> Do(string name)
        {
            var expiredStocksOnHold = _ctx.StocksOnHold.Where(x => x.ExpiryTime < DateTime.Now).ToList();

            if (expiredStocksOnHold.Count > 0)
            {
                foreach (var expiredStock in expiredStocksOnHold)
                {
                    var stock = _ctx.Stock.Where(x => expiredStock.StockId == x.Id).FirstOrDefault();
                    stock.Qty += expiredStock.Qty;
                }
                _ctx.RemoveRange(expiredStocksOnHold);
                await _ctx.SaveChangesAsync();
            }

            return _ctx.Products.Where(x => x.Name == name)
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
        }
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
