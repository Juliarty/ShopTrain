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

        public AddToCart(ApplicationDbContext ctx, ISession session)
        {
            _ctx = ctx;
            _session = session;
        }

        public async Task<bool> Do(Request request)
        {

            var stocksOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == _session.Id);
            var stockToHold = _ctx.Stock.FirstOrDefault(x => x.Id == request.StockId);

            if(stockToHold.Qty < request.Qty)
            {
                return false;
            }

            _ctx.StocksOnHold.Add(new StocksOnHold()
            {
                StockId = request.StockId,
                SessionId = _session.Id,
                Qty = request.Qty,
                ExpiryTime = DateTime.Now.AddMinutes(20)
            });

            stockToHold.Qty -= request.Qty;

            foreach (var stock in stocksOnHold)
            {
                stock.ExpiryTime = DateTime.Now.AddMinutes(20);
                //ToDo: extend cookie expiring time, or just keep it for session time... no.. it's not so convenient
            }

            await _ctx.SaveChangesAsync();

            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString("cart");
            if(!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            }

            if(cartList.Any(x => x.StockId == request.StockId))
            {
                cartList.Find(x => x.StockId == request.StockId).Qty += request.Qty;
            }
            else
            {
                var cartProduct = new CartProduct()
                {
                    Qty = request.Qty,
                    StockId = request.StockId
                };
                cartList.Add(cartProduct);
            }
            stringObject = JsonConvert.SerializeObject(cartList);

            _session.SetString("cart", stringObject);

            return true;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
    }
}
