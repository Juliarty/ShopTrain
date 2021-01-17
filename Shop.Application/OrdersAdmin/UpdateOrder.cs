using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shop.Domain.Enums;
using System.Threading.Tasks;

namespace Shop.Application.OrdersAdmin
{
    public class UpdateOrder
    {

        private ApplicationDbContext _ctx;

        public UpdateOrder(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> DoAsync(int orderId)
        {
            var order = _ctx.Orders
                .FirstOrDefault(x => x.Id == orderId);

            if (order == null) return true;

            switch (order.OrderStatus)
            {
                case OrderStatus.Pending: order.OrderStatus = OrderStatus.Packed; break;
                case OrderStatus.Packed: order.OrderStatus = OrderStatus.Shipped; break;
                case OrderStatus.Shipped: order.OrderStatus = OrderStatus.Done; break;
            }

            return await _ctx.SaveChangesAsync() > 0;
        }

    }
}
