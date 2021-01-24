using Shop.Database;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shop.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private ApplicationDbContext _ctx;
        private ISession _session;

        public AddToCart(ApplicationDbContext ctx, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = ctx;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public async Task<bool> DoAsync(Request request)
        {
            var stocksOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == _session.Id);
                // User is active so we postpone the moment when the goods he chose is expired
            foreach (var stock in stocksOnHold)
            {
                // ToDo: User can change infinitely, but shoukld be mechanism to avoid this
                stock.ExpiryTime = DateTime.Now.AddMinutes(20);
                // ToDo: extend cookie expiring time, or just keep it for session time... no.. it's not so convenient
            }
            
            var chosenStock = _ctx.Stock.FirstOrDefault(x => x.Id == request.StockId);
            var stockOnHold = _ctx.StocksOnHold.FirstOrDefault(x => x.SessionId == _session.Id && x.StockId == request.StockId);

            if(request.Qty > 0)
            {
                if(chosenStock.Qty < request.Qty) return false;
            }
            else
            {
                // we leave one element always in a cart (a user should remove it manually)
                if(stockOnHold.Qty - 1 < -request.Qty) return false;
            }

            // Prepare good for being ordered
            if (stockOnHold == null)
            {
                _ctx.StocksOnHold.Add(new StocksOnHold()
                {
                    StockId = request.StockId,
                    SessionId = _session.Id,
                    Qty = request.Qty,
                    ExpiryTime = DateTime.Now.AddMinutes(20)
                });
            }
            else
            {
                stockOnHold.Qty += request.Qty;
            }

            chosenStock.Qty -= request.Qty;

        

            await _ctx.SaveChangesAsync();

            ChangeCookie(_session, request.StockId, request.Qty);

            return true;
        }


        private static void ChangeCookie(ISession session, int stockId, int qty)
        {
            var cartList = new List<CartStock>();
            var stringObject = session.GetString("cart");
            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartStock>>(stringObject);
            }

            if (cartList.Any(x => x.StockId == stockId))
            {
                cartList.Find(x => x.StockId == stockId).Qty += qty;
            }
            else
            {
                var cartProduct = new CartStock()
                {
                    Qty = qty,
                    StockId = stockId
                };
                cartList.Add(cartProduct);
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            session.SetString("cart", stringObject);
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
    }
}
