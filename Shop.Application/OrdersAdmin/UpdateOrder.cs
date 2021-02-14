using System;
using Shop.Domain.Enums;
using System.Threading.Tasks;
using Shop.Domain.Infrastructure;

namespace Shop.Application.OrdersAdmin
{
    [Service]
    public class UpdateOrder
    {

        private IOrderManager _orderManager;

        public UpdateOrder(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public async Task<bool> DoAsync(int orderId)
        {
            var order = _orderManager.GetOrderById(orderId, x => x);

            OrderStatus newStatus;
            switch (order.OrderStatus)
            {
                case OrderStatus.Pending: newStatus = OrderStatus.Packed; break;
                case OrderStatus.Packed: newStatus = OrderStatus.Shipped; break;
                case OrderStatus.Shipped: newStatus = OrderStatus.Done; break;
                case OrderStatus.Done: return true;
                default: throw new Exception($"Unexpected order status {Enum.GetName(typeof(OrderStatus), order.OrderStatus)}");
            }

            return await _orderManager.UpdateOrderStatus(orderId, newStatus);
        }

    }
}
