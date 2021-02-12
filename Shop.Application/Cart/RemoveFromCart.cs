using Shop.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Cart
{
    public class RemoveFromCart
    {
        private ISessionManager _sessionManager;
        private IStockManager _stockManager;

        public RemoveFromCart(IStockManager stockManager, ISessionManager sessionManager)
        {
            _stockManager = stockManager;
            _sessionManager = sessionManager;
        }

        public async Task<bool> DoAsync(Request request)
        {
            var cartList = _sessionManager.GetCartItems(x => x).ToList();

            var cartItem = cartList.FirstOrDefault(x => x.StockId == request.StockId);
            if (cartItem == null) return true;

            if (request.RemoveAll)
            {
                cartList.Remove(cartItem);
                await _stockManager.ReleaseStockOnHoldAsync(request.StockId, cartItem.Qty, _sessionManager.GetId());
            }
            else
            {
                // we wanna keep one product left, so customer can change his mind while trying to find 'remove' button
                if (cartItem.Qty - request.Qty <= 0)
                {
                    cartItem.Qty = 1;
                    await _stockManager.ReleaseStockOnHoldAsync(request.StockId, cartItem.Qty - 1, _sessionManager.GetId());
                }
                else
                {
                    cartItem.Qty -= request.Qty;
                    await _stockManager.ReleaseStockOnHoldAsync(request.StockId, request.Qty, _sessionManager.GetId());
                }
            }
            

            _sessionManager.SetCartItems(cartList);

            return true;
        }



        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
            public bool RemoveAll { get; set; }
        }
    }
}
