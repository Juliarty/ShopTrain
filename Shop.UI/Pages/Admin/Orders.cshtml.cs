using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.OrdersAdmin;
using Shop.Database;
using Shop.Domain.Enums;

namespace Shop.UI.Pages.Admin
{
    public class OrdersModel : PageModel
    {
        private ApplicationDbContext _ctx;

        public OrdersModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public GetOrders.Response Orders { get; set; }
        public IActionResult OnGet()
        {
            Orders = new GetOrders(_ctx).Do((int)OrderStatus.Pending);
            return Page();
        }
    }
}
