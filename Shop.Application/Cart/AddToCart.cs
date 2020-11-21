using Shop.Database;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shop.Domain.Models;
using System.Linq;

namespace Shop.Application.Cart
{
    public class AddToCart
    {
        private ISession _session;

        public AddToCart(ISession session)
        {
            _session = session;
        }

        public void Do(Request request)
        {
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
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Qty { get; set; }
        }
    }
}
