using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Database;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    public class RemoveItem
    {


        private ApplicationDbContext _ctx;
        private ISession _session;

        public RemoveItem(ApplicationDbContext ctx, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = ctx;
            _session = httpContextAccessor.HttpContext.Session;
        }


        public async Task<bool> DoAsync(int stockId)
        {
            var chosenStock = _ctx.Stock.FirstOrDefault(x => x.Id == stockId);


            var cartList = new List<CartStock>();
            var stringObject = _session.GetString("cart");


            // Get a user cart
            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartStock>>(stringObject);
            }
            else // no cart, no products on hold
            {
                var stocksOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == _session.Id);
                if(stocksOnHold != null)
                {
                    // It shouldn't work this way
                    throw new Exception("StockOnHold is corrupted");
                    // The cart is already empty, but some products are hold in the StockOnHold
                }
                return true;
            }
            
            // Remove the stock from the user cart
            var stockFromCart = cartList.FirstOrDefault(x => x.StockId == stockId);
            if (stockFromCart == null)
            {
                // there is no such stock in the cart
                // we can leave checking stockOnHold, because it will return to the normal state soon
                return true;
            }
            
            var qtyToRemove = stockFromCart.Qty;
            cartList.Remove(stockFromCart);

            var stockToUnhold = _ctx.StocksOnHold.FirstOrDefault(x => x.SessionId == _session.Id && x.StockId == stockId);

            if(stockToUnhold == null || stockToUnhold.Qty < qtyToRemove)
            {
                throw new Exception("StockOnHold is corrupted");
            }

            stockToUnhold.Qty -= qtyToRemove;

            if(stockToUnhold.Qty <= 0)
            {
                _ctx.StocksOnHold.Remove(stockToUnhold);
            }

            chosenStock.Qty += qtyToRemove;

            await _ctx.SaveChangesAsync();
  
            // Save cookie
            stringObject = JsonConvert.SerializeObject(cartList);
            _session.SetString("cart", stringObject);
            return true;
        }
        
    }


    
}
