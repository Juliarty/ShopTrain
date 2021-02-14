using Shop.Domain.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.ProductsAdmin
{
    [Service]
    public class DeleteProduct
    {
        private readonly IProductManager _productManager;

        public DeleteProduct(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<bool> Do(int id)
        {
            return await _productManager.RemoveProductAsync(id);
            
        }
    }
}
