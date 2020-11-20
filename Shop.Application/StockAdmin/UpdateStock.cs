using Shop.Database;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Domain.Models;

namespace Shop.Application.StockAdmin
{
    public class UpdateStock
    {
        private ApplicationDbContext _ctx;

        public UpdateStock(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response> Do(Request request)
        {
            var stock = new List<Stock>();

            foreach(var el in request.Stock)
            {
                stock.Add(new Stock
                {
                    Id = el.Id,
                    Description = el.Description,
                    Qty = el.Qty,
                    ProductId = el.ProductId
                });
            }
            _ctx.Stock.UpdateRange(stock);

            await _ctx.SaveChangesAsync();

            return new Response
            {
                Stock = request.Stock
            };
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
            public int ProductId { get; set; }
        }
        public class Request
        {
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
        public class Response
        {
            public IEnumerable<StockViewModel> Stock { get; set; }

        }
    }
}
