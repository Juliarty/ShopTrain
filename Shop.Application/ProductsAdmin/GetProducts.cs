using Shop.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shop.Application.ProductsAdmin
{
    public class GetProducts
    {
        private ApplicationDbContext _context;

        public GetProducts(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Response> Do() =>
            _context.Products.ToList().Select(x => new Response
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
            public string ValueStrRubles { get => $"\x20bd{ValueInRubles}"; }
        }
    }

}
