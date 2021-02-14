using Shop.Domain.Infrastructure;
using System.Collections.Generic;

namespace Shop.Application.ProductsAdmin
{
    [Service]
    public class GetProducts
    {
        private readonly IProductManager _productManager;

        public GetProducts(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IEnumerable<Response> Do() => _productManager.GetProducts(x => true, x => new Response
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            ValueInRubles = x.Value
        });

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal ValueInRubles { get; set; }
            public string ValueStrRubles { get => $"\x20bd{ValueInRubles:N2}"; }
        }
    }

}
