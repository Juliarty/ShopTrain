using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    [Service]
    public class GetProduct
    {
        private IProductManager _productManager;
        private IStockManager _stockManager;

        public GetProduct(IProductManager productManager, IStockManager stockManager)
        {
            _productManager = productManager;
            _stockManager = stockManager;
        }

        public async Task<Response> DoAsync(int id)
        {
            await _stockManager.RetrieveExpiredStocksOnHoldAsync();

            return _productManager.GetProductById(id, x => new Response
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
            });
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
