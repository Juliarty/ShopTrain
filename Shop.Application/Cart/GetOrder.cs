using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Database;
using System.Collections.Generic;
using System.Linq;
namespace Shop.Application.Cart
{
    public class GetOrder
    {
        private ISessionManager _sessionManager;

        public GetOrder(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public Response Do()
        {
            var listOfProducts = _sessionManager.GetCartItems(x => new Product()
                {
                    ProductId = x.ProductId,
                    StockId = x.StockId,
                    Value = x.ValueInRubles, // Do not lose cents!
                    Qty = x.Qty
                });

            var customerInformation = _sessionManager.GetCustomerInformation();

            return new Response()
            {
                Products = listOfProducts,
                CustomerInformation = new CustomerInformation
                {
                    FirstName = customerInformation.FirstName,
                    LastName = customerInformation.LastName,
                    Email = customerInformation.Email,
                    PhoneNumber = customerInformation.PhoneNumber,
                    Address1 = customerInformation.Address1,
                    Address2 = customerInformation.Address2,
                    City = customerInformation.City,
                    PostCode = customerInformation.PostCode
                }
            };

        }
        public class Response
        {
            public IEnumerable<Product> Products { get; set; }
            public CustomerInformation CustomerInformation { get; set; }
            public string TotalChargeStrRubles => GetTotalCharge().GetRubPrice();
            public decimal GetTotalCharge() => Products.Sum(x => x.Value * x.Qty);
        }


        public class Product
        {
            public int ProductId { get; set; }
            public int Qty { get; set; }
            public int StockId { get; set; }
            public decimal Value { get; set; }

        }
        public class CustomerInformation
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
