using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{

    public class StockManager : IStockManager
    {
        private ApplicationDbContext _ctx;
        public StockManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> CreateStockAsync(Stock stock)
        {
            _ctx.Stock.Add(stock);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveStockAsync(int stockId)
        {
            var stock = _ctx.Stock.FirstOrDefault(x => x.Id == stockId);
            _ctx.Stock.Remove(stock);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStockRangeAsync(IEnumerable<Stock> stocks)
        {
            _ctx.Stock.UpdateRange(stocks);
            return await _ctx.SaveChangesAsync() > 0;
        }

   
        public bool EnoughStock(int stockId, int qty)
        {
            var stock = _ctx.Stock.FirstOrDefault(x => x.Id == stockId);

            return stock == null ? false : stock.Qty >= qty;
        }

        public Stock GetStockWithProduct(int stockId)
        {

            return _ctx.Stock
                .Where(x => x.Id == stockId)
                .Include(x => x.Product)
                .FirstOrDefault();
        }

        public async Task PutStockOnHoldAsync(int stockId, int qty, string sessionId)
        {

            // begin Transaction

            // update Stock set qty = qty + {0} where id = {1}
            _ctx.Stock.FirstOrDefault(x => x.Id == stockId).Qty -= qty;

            var allSessionStocksOnHold = _ctx.StocksOnHold
                .Where(x => x.SessionId == sessionId)
                .ToList();
            foreach (var stock in allSessionStocksOnHold)
            {
                // ToDo: User can change infinitely, but shoukld be mechanism to avoid this
                stock.ExpiryTime = DateTime.Now.AddMinutes(20);
                // ToDo: extend cookie expiring time, or just keep it for session time... no.. it's not so convenient
            }

            var stockOnHoldToChange = allSessionStocksOnHold.FirstOrDefault(x => x.StockId == stockId);


            // Prepare the good for being ordered

            //select count(*) from StocksOnHold where StockId = {0} and sessionId = {1}
            if (allSessionStocksOnHold.Any(x => x.Id == stockId))
            {
                // update StocksOnHold set qty = qty + {0}
                // where StockId = {1} and sessionID = {2}
                allSessionStocksOnHold.Find(x => x.Id == stockId).Qty += qty;
            }
            else
            {
                // insert into StocksOnHold (stockId, sessioniD, qty, ExpiryDate)
                // values {0}, {1}, {2}, {3}
                _ctx.StocksOnHold.Add(new StocksOnHold()
                {
                    StockId = stockId,
                    SessionId = sessionId,
                    Qty = qty,
                    ExpiryTime = DateTime.Now.AddMinutes(20)
                });
            }
            await _ctx.SaveChangesAsync();
            // end transaction
        }

        public async Task ReleaseStockOnHoldAsync(int stockId, int qty, string sessionId)
        {
            var stockToUnhold = _ctx.StocksOnHold.FirstOrDefault(x => x.SessionId == sessionId && x.StockId == stockId);

            if (stockToUnhold == null) return;

            stockToUnhold.Qty -= qty;

            if (stockToUnhold.Qty <= 0)
            {
                _ctx.StocksOnHold.Remove(stockToUnhold);
            }

            _ctx.Stock.FirstOrDefault(x => x.Id == stockId).Qty += qty;

            await _ctx.SaveChangesAsync();

            return;
        }

        public async Task ReleaseStockOnHoldAsync(string sessionId)
        {
            var stockOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == sessionId).ToList();

            _ctx.StocksOnHold.RemoveRange(stockOnHold);

            await _ctx.SaveChangesAsync();
        }


        public async Task<bool> RetrieveExpiredStocksOnHoldAsync()
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


        public IEnumerable<TResult> GetStocksForProduct<TResult>(int productId, Func<Stock, TResult> selector) =>
            _ctx.Stock
            .Include(x => x.Product)
            .Where(x => x.Product.Id == productId)
            .Select(selector);
            
    }
}
