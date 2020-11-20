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

        public Response Do(int id) =>
            _context.Products.ToList().Where(x => x.Id == id).Select(x => new Response
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Value = x.Value.ToString()
            }).FirstOrDefault();
        public class Response
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
        }
    }


 

}
