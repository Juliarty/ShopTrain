using Shop.Application.Infrastructure;
using Shop.Database;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    public class RemoveItem
    {
        private ApplicationDbContext _ctx;
        private ISessionManager _sessionManager;

        public RemoveItem(ApplicationDbContext ctx, ISessionManager sessionManager)
        {
            _ctx = ctx;
            _sessionManager = sessionManager;
        }


        public async Task<bool> DoAsync(int stockId)
        {
            var chosenStock = _ctx.Stock.FirstOrDefault(x => x.Id == stockId);


            var cartList = _sessionManager.GetCartItems().ToList();


            // Get a user cart
            if (cartList.Count == 0) // no products on hold
            {
                var stocksOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == _sessionManager.GetId());
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

            var stockToUnhold = _ctx.StocksOnHold.FirstOrDefault(x => x.SessionId == _sessionManager.GetId() && x.StockId == stockId);

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
            //stringObject = JsonConvert.SerializeObject(cartList);
            //SetString("cart", stringObject);
            _sessionManager.SetCartItems(cartList.Select(x=> new AddToCart.Request()
            {
                StockId = x.StockId,
                Qty = x.Qty
            }));

            return true;
        }
        
    }


    
}
