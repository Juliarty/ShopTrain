using Shop.Database;
using System;
using Shop.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Infrastructure;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private ApplicationDbContext _ctx;
        private ISessionManager _sessionManager;

        public AddToCart(ApplicationDbContext ctx, ISessionManager sessionManager)
        {
            _ctx = ctx;
            _sessionManager = sessionManager;
        }

        public async Task<bool> DoAsync(Request request)
        {
            var stocksOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == _sessionManager.GetId());
                // User is active so we postpone the moment when the goods he chose is expired
            foreach (var stock in stocksOnHold)
            {
                // ToDo: User can change infinitely, but shoukld be mechanism to avoid this
                stock.ExpiryTime = DateTime.Now.AddMinutes(20);
                // ToDo: extend cookie expiring time, or just keep it for session time... no.. it's not so convenient
            }
            
            var chosenStock = _ctx.Stock.FirstOrDefault(x => x.Id == request.StockId);
            var stockOnHold = _ctx.StocksOnHold.FirstOrDefault(x => x.SessionId == _sessionManager.GetId() && x.StockId == request.StockId);

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
                    SessionId = _sessionManager.GetId(),
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

            ChangeCookie(_sessionManager, request.StockId, request.Qty);

            return true;
        }


        private static void ChangeCookie(ISessionManager sessionManager, int stockId, int qty)
        {
            var cartList = sessionManager.GetCartItems().Select(x => new Request()
            {
                StockId = x.StockId,
                Qty = x.Qty
            }).ToList();

            if (cartList.Any(x => x.StockId == stockId))
            {
                var cartStockToChange = cartList.FirstOrDefault(x => x.StockId == stockId);
                if(cartStockToChange != null)
                {
                    cartStockToChange.Qty += qty;
                }             
            }
            else
            {
                var cartProduct = new Request()
                {
                    Qty = qty,
                    StockId = stockId
                };
                cartList.Add(cartProduct);
            }

            sessionManager.SetCartItems(cartList);
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
    }
}
