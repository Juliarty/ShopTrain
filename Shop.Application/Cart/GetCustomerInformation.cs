using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Cart
{
    public class GetCustomerInformation
    {
        private ISession _session;

        public GetCustomerInformation(ISession session)
        {
            _session = session;
        }

        public Response Do()
        {
            var stringObject = _session.GetString("customer-info");
            if (string.IsNullOrEmpty(stringObject))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Response>(stringObject);
        }

        public class Response
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public int City { get; set; }
            public int PostCode { get; set; }

        }
    }
}
