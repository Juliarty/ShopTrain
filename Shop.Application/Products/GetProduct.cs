using Shop.Application.ViewModels;
using Shop.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application
{
    public class GetProduct
    {
        private ApplicationDbContext _context;

        public GetProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductViewModel Do(int id) =>
            _context.Products.ToList().Where(x => x.Id == id).Select(x => new ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = x.Value
            }).FirstOrDefault();

    }


}
