using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.OrdersAdmin
{
    [Service]
    public class GetOrder
    {
        private IOrderManager _orderManger;

        public GetOrder(IOrderManager orderManger)
        {
            _orderManger = orderManger;
        }

        public Response Do(int orderId) => _orderManger.GetOrderById(orderId, _projection);


        private static Func<Order, Response> _projection = (order) => new Response()
        {
            Id = order.Id,
            OrderRef = order.OrderRef,
            FirstName = order.FirstName,
            LastName = order.LastName,
            Email = order.Email,
            PhoneNumber = order.PhoneNumber,
            Address1 = order.Address1,
            Address2 = order.Address2,
            City = order.City,
            PostCode = order.PostCode,
            OrderStatus = order.OrderStatus,
            Products = order.OrderStocks.Select(y => new Product
            {
                Name = y.Stock.Product.Name,
                Description = y.Stock.Product.Description,
                Value = $"${y.Stock.Product.Value:N2}",
                Qty = y.Qty,
                StockDescription = y.Stock.Description,
                OverallValue = $"${y.Stock.Product.Value * y.Qty:N2}"
            }),
            TotalValue = $"${order.OrderStocks.Sum(y => y.Stock.Product.Value * y.Qty):N2}"
        };

        public class Response
        {
            public int Id { get; set; }
            public string OrderRef { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }

            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
            public OrderStatus OrderStatus { get; set; }
            public IEnumerable<Product> Products { get; set; }
            public string TotalValue { get; set; }
        }
        public class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public int Qty { get; set; }
            public string StockDescription { get; set; }

            public string OverallValue { get; set; }
        }
    }
}
