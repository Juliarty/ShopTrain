﻿using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Orders
{
    [Service]
    public class CreateOrder
    {
        private IOrderManager _orderManager;
        private IStockManager _stockManager;

        public CreateOrder(IOrderManager orderManager, IStockManager stockManager)
        {
            _orderManager = orderManager;
            _stockManager = stockManager;
        }

        public async Task<bool> Do(Request request)
        {

            var order = new Order()
            {
                OrderRef = _orderManager.CreateOrderReference(),
                StripeRef = request.StripeReference,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                PostCode = request.PostCode,
                OrderStatus = OrderStatus.Pending,
                OrderStocks = request.Stocks.Select(x => new OrderStock() { StockId = x.StockId, Qty = x.Qty }).ToList()
            };

            var success = await _orderManager.CreateOrderAsync(order);

            if (!success) return false;
            
            await _stockManager.ReleaseStockOnHoldAsync(request.SessionId);

            return true;
            
        }

        public class Request
        {
            public string StripeReference { get; set; }
            public string SessionId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
            public OrderStatus OrderStatus { get; set; }

            public List<Stock> Stocks { get; set; }
        }


        public class Stock
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
    }
}
