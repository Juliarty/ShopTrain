using Shop.Domain.Infrastructure;
using Shop.Domain.Models;

namespace Shop.Application.Cart
{
    [Service]
    public class AddCustomerInformation
    {

        private ISessionManager _sessioManager;

        public AddCustomerInformation(ISessionManager sessionManager)
        {
            _sessioManager = sessionManager;
        }

        public void Do(Request request)
        {
            _sessioManager.SetCustomerInformation(new CustomerInformation()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                PostCode = request.PostCode,

            });

        }

        public class Request
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string PostCode { get; set; }

        }
    }
}
