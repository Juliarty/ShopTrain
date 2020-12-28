using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Application.Cart
{
    public class GetCart
    {

        private ISession _session;
        private ApplicationDbContext _ctx;

        public GetCart(ISession session, ApplicationDbContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public IEnumerable<Response> Do()
        {
            // TODO: account for multiple items in the cart

            var stringObject = _session.GetString("cart");
            if (string.IsNullOrEmpty(stringObject))
            {
                return Enumerable.Empty<Response>();
            }

            var cartList = JsonConvert.DeserializeObject<List<Response>>(stringObject);
            var responseList = new List<Response>();
            
            var response = _ctx.Stock
            .Include(x => x.Product)
            .AsEnumerable()
            .Where(x => cartList.Any(y => x.Id == y.StockId))
            .Select(x => new Response()
            {
                Name = x.Product.Name,
                ValueInRubles = x.Product.Value,
                StockId = x.Id,
                Qty = cartList.FirstOrDefault(y => y.StockId == x.Id).Qty
            })
            .ToList();

            return response;
        }

        public class Response
        {
            public string Name { get; set; }
            public decimal ValueInRubles { get; set; }
            public string ValueStrRubles { get => $"\x20BD{ValueInRubles:N2}"; }
            public int Qty { get; set; }
            public int StockId { get; set; }
            
        }
    }
}
