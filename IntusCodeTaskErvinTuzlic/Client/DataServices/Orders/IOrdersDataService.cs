using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using IntusCodeTaskErvinTuzlic.Shared.DTO;

namespace IntusCodeTaskErvinTuzlic.Client.DataServices.Orders;

public interface IOrdersDataService
{
    Task<List<Order>> GetAll();

    Task<Order?> Get(int orderId);

    Task<OrderUpsertResponse> Upsert(OrderUpsertRequest request);

    Task Delete(int id);
}
