using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Cart;
using Shop.Application.OrdersAdmin;
using Shop.Application.Users;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shop.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Shop.Application;
namespace Shop.Application
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {

            @this.AddTransient<GetOrders>();
            @this.AddTransient<OrdersAdmin.GetOrder>();
            @this.AddTransient<UpdateOrder>();
            
            @this.AddTransient<AddToCart>();
            @this.AddTransient<RemoveItem>();
            @this.AddTransient<GetCart>();

            @this.AddHttpContextAccessor();
            @this.AddTransient<CreateUser>();

            IServiceCollection s;
           
            return @this;
        }
    }
}
