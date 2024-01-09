using IntusCodeTaskErvinTuzlic.Client.DataServices.Orders;

namespace IntusCodeTaskErvinTuzlic.Client;

public class DependencyInjector
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<IOrdersDataService, OrdersDataService>();
    }
}
