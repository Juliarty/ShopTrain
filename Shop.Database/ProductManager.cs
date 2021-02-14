using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public class ProductManager : IProductManager
    {
        private ApplicationDbContext _ctx;

        public ProductManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> CreateProductAsync(Product product)
        {
            _ctx.Products.Add(product);
            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveProductAsync(int productId)
        {
            var product = _ctx.Products.FirstOrDefault(x => x.Id == productId);
            _ctx.Products.Remove(product);
            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            _ctx.Products.Update(product);
            return await _ctx.SaveChangesAsync() > 0;
        }


        public IEnumerable<TResult> GetProducts<TResult>(Func<Product, bool> condition, Func<Product, TResult> selector) =>
            _ctx.Products
            .Include(x => x.Stock)
            .ToList()
            .Where(x => condition(x))
            .Select(selector);

        public TResult GetProductById<TResult>(int productId, Func<Product, TResult> selector) =>
            GetProducts((product) => product.Id == productId, selector)
            .FirstOrDefault();

    }

}
