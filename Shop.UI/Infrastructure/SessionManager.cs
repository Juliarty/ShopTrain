using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shop.Application.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Cart;
namespace Shop.UI.Infrastructure
{
    public class SessionManager: ISessionManager
    {
        private ISession _session;
        private readonly string _customerInfoCookieName = "customer-info";
        private readonly string _cartCookieName = "cart";
        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }


        #region ISessionManager
        public string GetId() => _session.Id;
        public void SetCartItems(IEnumerable<AddToCart.Request> items)
        {
            var stringObject = JsonConvert.SerializeObject(items.ToList());
            _session.SetString(_cartCookieName, stringObject);

        }
        public IEnumerable<GetCart.Response> GetCartItems()
        {
            var stringObject = _session.GetString(_cartCookieName);

            if (string.IsNullOrEmpty(stringObject)) return Enumerable.Empty<GetCart.Response>();

            return JsonConvert.DeserializeObject<List<GetCart.Response>>(stringObject);
        }

        public CustomerInformation GetCustomerInformation()
        {
            var stringObject = _session.GetString(_customerInfoCookieName);

            if (string.IsNullOrEmpty(stringObject)) return null;

            return JsonConvert.DeserializeObject<CustomerInformation>(stringObject);
        }
        public void SetCustomerInformation(CustomerInformation customerInformation)
        {
            var stringObject = JsonConvert.SerializeObject(customerInformation);
            _session.SetString(_customerInfoCookieName, stringObject);
        }
        #endregion
    }
}
