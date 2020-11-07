using Shop.Application.ViewModels;
using Shop.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application
{
    public class GetProducts
    {
        private ApplicationDbContext _context;

        public GetProducts(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _context.Products.ToList().Select(x => new ViewModels.ProductViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Value = x.Value
            });

    }


}
