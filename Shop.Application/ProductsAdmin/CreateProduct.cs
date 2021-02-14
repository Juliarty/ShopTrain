using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin
{
    [Service]
    public class CreateProduct
    {
        private readonly IProductManager _productManager;

        public CreateProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<Response> DoAsync(Request productViewModel)
        {
            var product = new Product()
            {
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Value = Utils.GetDecimal(productViewModel.Value)
            };

            var success = await _productManager.CreateProductAsync(product);

            if (!success) throw new System.Exception("Couldn't create a product.");

            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Value = product.Value.ToString()
            };
        }

        public class Request
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }

        }

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }

        }

    }


}
