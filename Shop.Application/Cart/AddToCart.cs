using Shop.Database;
using System;
using Shop.Domain.Models;
using System.Linq;
using System.Threading.Tasks;
using Shop.Domain.Infrastructure;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private ISessionManager _sessionManager;
        private IStockManager _stockManager;


        public AddToCart(IStockManager stockManager, ISessionManager sessionManager)
        {
            _stockManager = stockManager;
            _sessionManager = sessionManager;
        }

        public async Task<bool> DoAsync(Request request)
        {
            if (!_stockManager.EnoughStock(request.StockId, request.Qty)) return false;

            await _stockManager.PutStockOnHoldAsync(request.StockId, request.Qty, _sessionManager.GetId());

            var cartList = _sessionManager.GetCartItems(x => x).ToList();

            var cartItem = cartList.FirstOrDefault(x => x.StockId == request.StockId);
            if (cartItem != null)
            {
                cartItem.Qty += request.Qty;
            }
            else
            {
                var stock = _stockManager.GetStockWithProduct(request.StockId);
                cartList.Add(new CartProduct()
                {
                    Qty = request.Qty,
                    StockId = request.StockId,
                    ProductName = stock.Product.Name,
                    ValueInRubles = stock.Product.Value,
                    ProductId = stock.ProductId
                });
            }
            _sessionManager.SetCartItems(cartList);
            return true;
        }



        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
    }
}
