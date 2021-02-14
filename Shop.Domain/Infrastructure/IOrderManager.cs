using Shop.Domain.Enums;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IOrderManager
    {
        Task<bool> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderStatus(int orderId, OrderStatus newStatus);


        IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector);
        TResult GetOrderById<TResult>(int orderId, Func<Order, TResult> selector);
        TResult GetOrderByReference<TResult>(string reference, Func<Order, TResult> selector);

        string CreateOrderReference();
        
    }
}
