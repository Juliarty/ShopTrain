using Shop.Domain.Infrastructure;

namespace Shop.Application.ProductsAdmin
{
    [Service]
    public class GetProduct
    {
        private readonly IProductManager _productManager;

        public GetProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public Response Do(int id) => _productManager.GetProductById(id, x => new Response
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Value = x.Value.ToString()
            });

        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
        }
    }




}
