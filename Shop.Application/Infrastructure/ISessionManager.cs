using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Infrastructure
{
    public interface ISessionManager
    {
        public string GetId();
        public void SetCartItems(IEnumerable<Cart.AddToCart.Request> items);
        public IEnumerable<Cart.GetCart.Response> GetCartItems();

        public CustomerInformation GetCustomerInformation();
        public void SetCustomerInformation(CustomerInformation customerInformation);

    }
}
