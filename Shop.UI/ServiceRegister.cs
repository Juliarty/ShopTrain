using Microsoft.Extensions.DependencyInjection;
using Shop.Application;
using Shop.Domain.Infrastructure;
using Shop.UI.Infrastructure;
using System.Linq;
using System.Reflection;

namespace Shop.UI
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {
            var sericeType = typeof(Service);
            var definedTypes = sericeType.Assembly.DefinedTypes;

            var services = definedTypes.Where(x => x.GetTypeInfo().GetCustomAttribute<Service>() != null); ;

            foreach (var service in services)
            {
                @this.AddTransient(service);
            }

            @this.AddScoped<ISessionManager, SessionManager>();
            @this.AddTransient<IStockManager, StockManager>();
            @this.AddTransient<IProductManager, ProductManager>();
            @this.AddTransient<IOrderManager, OrderManager>();

            return @this;
        }
    }
}
