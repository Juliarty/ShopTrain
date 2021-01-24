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

        public async Task<Response> DoAsync(string name)
        {
            await new RemoveExpiredStocksOnHold(_ctx).DoAsync();

            return _ctx.Products.Where(x => x.Name == name)
                .Include(x => x.Stock)
                .Select(x => new Response
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ValueInRubles = x.Value,
                    Stock = x.Stock.Select(y => new StockViewModel
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Qty = y.Qty
                    }).ToList()
                }).FirstOrDefault();
        }
        
        
        
        
        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal ValueInRubles { get; set; }
            public string ValueStrRubles { get => $"\x20bd{ValueInRubles}"; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class StockViewModel 
        {
            public int Id;
            public string Description;
            public int Qty;
        }

    }
}
