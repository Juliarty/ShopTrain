using Shop.Domain.Models;
using System;
using System.Collections.Generic;

namespace Shop.Domain.Infrastructure
{
    public interface ISessionManager
    {
        string GetId();
        void SetCartItems(IEnumerable<CartProduct> items);
        IEnumerable<TResult> GetCartItems<TResult>(Func<CartProduct, TResult> selector);

        CustomerInformation GetCustomerInformation();
        void SetCustomerInformation(CustomerInformation customerInformation);

        void ClearCart();
    }
}
