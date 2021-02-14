using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IProductManager
    {
        IEnumerable<TResult> GetProducts<TResult>(Func<Product, bool> condition, Func<Product, TResult> selector);
        TResult GetProductById<TResult>(int productId, Func<Product, TResult> selector);

        /// <summary>
        /// Adds a new product to the database. 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="product"></param>
        /// <param name="selector"></param>
        /// <returns>Returns a TResult object made from the product if the product was added successfully.
        /// Otherwise, return default(TResult)</returns>
        Task<bool> CreateProductAsync(Product product);
        Task<bool> RemoveProductAsync(int productId);

        Task<bool> UpdateProductAsync(Product product);

    }
}
