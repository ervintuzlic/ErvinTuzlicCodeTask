using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using IntusCodeTaskErvinTuzlic.Shared.DTO;

namespace IntusCodeTaskErvinTuzlic.Server.Services.Orders;

public interface IOrdersService
{
    Task<List<Order>> GetAll();

    Task<Order?> Get(int id);

    Task<OrderUpsertResponse> Upsert(OrderUpsertRequest request);

    Task Delete(int id);
}
