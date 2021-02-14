using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Enums;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public class OrderManager : IOrderManager
    {
        private ApplicationDbContext _ctx;

        public OrderManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            _ctx.Orders.Add(order);
            return await _ctx.SaveChangesAsync() > 0;
        }


        private IEnumerable<TResult> GetOrders<TResult>(Func<Order, bool> condition, Func<Order, TResult> selector) =>
            _ctx.Orders
              .Where(x => condition(x))
              .Include(x => x.OrderStocks)
                .ThenInclude(x => x.Stock)
                    .ThenInclude(x => x.Product)
              .Select(selector);

        public IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector) =>
            GetOrders((order) => order.OrderStatus == status, selector);

        public TResult GetOrderByReference<TResult>(string reference, Func<Order, TResult> selector) =>
            GetOrders((order) => order.OrderRef == reference, selector)
            .FirstOrDefault();

        public TResult GetOrderById<TResult>(int orderId, Func<Order, TResult> selector) =>
            GetOrders((order) => order.Id == orderId, selector)
            .FirstOrDefault();

        public async Task<bool> UpdateOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = _ctx.Orders
               .FirstOrDefault(x => x.Id == orderId);
            if (order == null) return true;

            order.OrderStatus = newStatus;
            return await _ctx.SaveChangesAsync() > 0;
        }

        public string CreateOrderReference()
        {
            var chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            var random = new Random();

            var result = new char[12];

            do
            {
                for (var i = 0; i < result.Length; ++i)
                {
                    result[i] = chars[random.Next(chars.Length)];
                }

            } while (_ctx.Orders.Any(x => x.OrderRef == new string(result)));

            return new string(result);
        }
    }
}
