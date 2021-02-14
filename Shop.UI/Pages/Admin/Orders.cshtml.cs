using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Application.OrdersAdmin;
using Shop.Database;
using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;

namespace Shop.UI.Pages.Admin
{
    public class OrdersModel : PageModel
    {
        private IOrderManager _orderManager;

        public OrdersModel(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public GetOrders.Response Orders { get; set; }
        public IActionResult OnGet()
        {
            Orders = new GetOrders(_orderManager).Do((int)OrderStatus.Pending);
            return Page();
        }
    }
}
