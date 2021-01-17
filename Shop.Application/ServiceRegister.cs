using Microsoft.Extensions.DependencyInjection;
using Shop.Application.OrdersAdmin;
using Shop.Application.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {

            @this.AddTransient<GetOrders>();
            @this.AddTransient<GetOrder>();
            @this.AddTransient<UpdateOrder>();
            
            @this.AddTransient<CreateUser>();


            return @this;
        }
    }
}
