using Shop.Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    [Service]
    public class GetProducts
    {
        private IProductManager _productManager;
        private IStockManager _stockManager;

        public GetProducts(IProductManager productManager, IStockManager stockManager)
        {
            _productManager = productManager;
            _stockManager = stockManager;
        }

        public async Task<IEnumerable<Response>> DoAsync()
        {
            await _stockManager.RetrieveExpiredStocksOnHoldAsync();

            return _productManager
                .GetProducts(x => true,
                x => new Response
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ValueInRubles = x.Value,
                    StockCount = x.Stock.Sum(y => y.Qty)
                });
        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal ValueInRubles { get; set; }
            public string ValueStrRubles { get => ValueInRubles.GetRubPrice(); }
            public int StockCount { get; set; }
        }
    }
}
