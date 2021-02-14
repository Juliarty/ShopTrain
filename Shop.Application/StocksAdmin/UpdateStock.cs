using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Domain.Models;
using Shop.Domain.Infrastructure;

namespace Shop.Application.StocksAdmin
{
    [Service]
    public class UpdateStock
    {
        private readonly IStockManager _stockManager;

        public UpdateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
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

            var success = await _stockManager.UpdateStockRangeAsync(stock);

            if (!success) throw new System.Exception("Couldn't update stock");

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
