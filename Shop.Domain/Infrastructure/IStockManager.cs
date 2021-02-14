using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{

    public interface IStockManager
    {
        Task<bool> CreateStockAsync(Stock stock);
        Task<bool> RemoveStockAsync(int stocktId);
        Task<bool> UpdateStockRangeAsync(IEnumerable<Stock> stocks);



        Stock GetStockWithProduct(int stockId);
        bool EnoughStock(int stockId, int qty);
        Task PutStockOnHoldAsync(int stockId, int qty, string sessionId);
        Task ReleaseStockOnHoldAsync(int stockId, int qty, string sessionId);
        Task ReleaseStockOnHoldAsync(string sessionId);


        IEnumerable<TResult> GetStocksForProduct<TResult>(int productId, Func<Stock, TResult> selector);

        Task<bool> RetrieveExpiredStocksOnHoldAsync();
    }
}
