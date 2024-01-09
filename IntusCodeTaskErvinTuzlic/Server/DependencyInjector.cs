using IntusCodeTaskErvinTuzlic.Server.Services.Orders;

namespace IntusCodeTaskErvinTuzlic.Server;

public class DependencyInjector
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<IOrdersService, OrdersService>();
    }
}