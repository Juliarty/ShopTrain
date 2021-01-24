using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class GetProducts
    {
        private ApplicationDbContext _ctx;

        public GetProducts(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<IEnumerable<Response>> DoAsync()
        {
            await new RemoveExpiredStocksOnHold(_ctx).DoAsync();

            return
            _ctx.Products
            .Include(x => x.Stock)
            .Select(x => new Response
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ValueInRubles = x.Value,
                StockCount = x.Stock.Sum(y => y.Qty)
            }).ToList();

        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal ValueInRubles { get; set; }
            public string ValueStrRubles { get => $"\x20bd{ValueInRubles:N2}"; }
            public int StockCount { get; set; }
        }
    }
}
