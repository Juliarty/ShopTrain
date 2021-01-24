using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    public class RemoveExpiredStocksOnHold
    {
        private ApplicationDbContext _ctx;

        public RemoveExpiredStocksOnHold(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> DoAsync()
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
            return true;
        }
    }
}
