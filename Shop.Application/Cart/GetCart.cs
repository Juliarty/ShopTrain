using Microsoft.EntityFrameworkCore;
using Shop.Application.Infrastructure;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    public class GetCart
    {

        private ISessionManager _sessionManager;
        private ApplicationDbContext _ctx;

        public GetCart(ISessionManager sessionManager, ApplicationDbContext ctx)
        {
            _sessionManager = sessionManager;
            _ctx = ctx;
        }

        public async Task<IEnumerable<Response>> DoAsync()
        {
            // TODO: account for multiple items in the cart

            //var stringObject = _sessionManager.GetString("cart");
            //if (string.IsNullOrEmpty(stringObject))
            //{
            //    return Enumerable.Empty<Response>();
            //}

            var cartList = _sessionManager.GetCartItems();
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
