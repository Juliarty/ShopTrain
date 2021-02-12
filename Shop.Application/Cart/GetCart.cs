using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
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

        public GetCart(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public IEnumerable<Response> Do()
        {
            // TODO: account for multiple items in the cart

            return _sessionManager.GetCartItems(x => new Response()
            {
                Name = x.ProductName,
                ValueInRubles = x.ValueInRubles,
                StockId = x.StockId,
                Qty = x.Qty
            });
           
            
        }

        public class Response
        {
            public string Name { get; set; }
            public decimal ValueInRubles { get; set; }
            public string ValueStrRubles => ValueInRubles.GetRubPrice();
            public int Qty { get; set; }
            public int StockId { get; set; }
            
        }
    }
}
