using System.Linq;
using System.Collections.Generic;
using Shop.Domain.Infrastructure;

namespace Shop.Application.StocksAdmin
{
    [Service]
    public class GetStock
    {
        private readonly IProductManager _productManager;

        public GetStock(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _productManager.GetProducts(x => true, x => new ProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Stock = x.Stock.Select(y => new StockViewModel
                    {
                        Id = y.Id,
                        Description = y.Description,
                        Qty = y.Qty
                    })
                });


        public class ProductViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }
}
