﻿using System;
using System.Threading.Tasks;
using Shop.Domain.Models;
using Shop.Domain.Infrastructure;

namespace Shop.Application.StocksAdmin
{
    [Service]
    public class CreateStock
    {
        private readonly IStockManager _stockManager;

        public CreateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public async Task<Response> Do(Request request)
        {
            var stock = new Stock()
            {
                Description = request.Description,
                ProductId = request.ProductId,
                Qty = request.Qty
            };

            var success = await _stockManager.CreateStockAsync(stock);

            if (!success) throw new Exception("Couldn't create a stock.");

            return new Response()
            {
                Id = stock.Id,
                Description = stock.Description,
                Qty = stock.Qty
            };
        }

        public class Request
        {
            public int ProductId { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }

        }
        public class Response
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Qty { get; set; }
        }
    }

   

}
