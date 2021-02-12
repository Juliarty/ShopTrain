using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Cart;
using Shop.Application.OrdersAdmin;
using Shop.Application.Products;
using Shop.Application.Users;

namespace Shop.Application
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {
            @this.AddTransient<GetProduct>();
            @this.AddTransient<GetProducts>();

            @this.AddTransient<GetOrders>();
            @this.AddTransient<OrdersAdmin.GetOrder>();
            @this.AddTransient<UpdateOrder>();
            
            @this.AddTransient<AddToCart>();
            @this.AddTransient<RemoveFromCart>();
            @this.AddTransient<GetCart>();

            
            @this.AddTransient<CreateUser>();

            return @this;
        }
    }
}
