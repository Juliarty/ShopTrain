﻿using Microsoft.EntityFrameworkCore;
using Shop.Application.Infrastructure;
using Shop.Database;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.Cart
{
    public class GetOrder
    {
        private ISessionManager _sessionManager;
        private ApplicationDbContext _ctx;

        public GetOrder(ISessionManager sessionManager, ApplicationDbContext ctx)
        {
            _sessionManager = sessionManager;
            _ctx = ctx;
        }

        public Response Do()
        {
            var cartList = _sessionManager.GetCartItems();

            var listOfProducts = _ctx.Stock
                .Include(x => x.Product)
                .AsEnumerable()
                .Where(x => cartList.Any(y => y.StockId == x.Id))
                .Select(x => new Product()
                {
                    ProductId = x.ProductId,
                    StockId = x.Id,
                    Value = x.Product.Value, // Do not lose cents!
                    Qty = cartList.FirstOrDefault(y => y.StockId == x.Id).Qty
                }).ToList();

            var customerInformation = _sessionManager.GetCustomerInformation();

            return new Response()
            {
                Products = listOfProducts,
                CustomerInformation = new CustomerInformation
                {
                    FirstName = customerInformation.FirstName,
                    LastName = customerInformation.LastName,
                    Email = customerInformation.Email,
                    PhoneNumber = customerInformation.PhoneNumber,
                    Address1 = customerInformation.Address1,
                    Address2 = customerInformation.Address2,
                    City = customerInformation.City,
                    PostCode = customerInformation.PostCode
                }
            };

        }
        public class Response
        {
            public IEnumerable<Product> Products { get; set; }
            public CustomerInformation CustomerInformation { get; set; }
            public string TotalChargeStrRubles => $"\x20BD{GetTotalCharge()}";
            public decimal GetTotalCharge() => Products.Sum(x => x.Value * x.Qty);
        }


        public class Product
        {
            public int ProductId { get; set; }
            public int Qty { get; set; }
            public int StockId { get; set; }
            public decimal Value { get; set; }

        }
        public class CustomerInformation
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }

            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }
        }
    }
}
