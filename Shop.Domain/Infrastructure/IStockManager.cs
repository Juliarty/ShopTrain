using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{

    public interface IStockManager
    {
        Stock GetStockWithProduct(int stockId);
        bool EnoughStock(int stockId, int qty);
        Task PutStockOnHoldAsync(int stockId, int qty, string sessionId);
        Task ReleaseStockOnHoldAsync(int stockId, int qty, string sessionId);

    }
}
