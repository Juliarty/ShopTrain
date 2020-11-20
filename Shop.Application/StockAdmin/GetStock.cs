using Shop.Database;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shop.Application.StockAdmin
{
    public class GetStock
    {
        private ApplicationDbContext _ctx;

        public GetStock(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ProductViewModel> Do()
        {
            var stock = _ctx.Products
                .Include(x => x.Stock)
                .Select
                (x => new ProductViewModel
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

            return stock;
        }

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
