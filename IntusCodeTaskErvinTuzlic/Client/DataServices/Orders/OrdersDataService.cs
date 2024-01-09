using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using IntusCodeTaskErvinTuzlic.Shared.DTO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace IntusCodeTaskErvinTuzlic.Client.DataServices.Orders;

public class OrdersDataService : IOrdersDataService
{
    private readonly HttpClient _httpClient;

    public OrdersDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Orders/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error while deleting Order");
        }
    }

    public async Task<Order?> Get(int orderId)
    {
        var result = await _httpClient.GetFromJsonAsync<Order>($"api/Orders/{orderId}");

        return result;
    }

    public async Task<List<Order>> GetAll()
    {
        var result = await _httpClient.GetFromJsonAsync<List<Order>>($"api/Orders");

        return result ?? new List<Order>();
    }

    public async Task<OrderUpsertResponse> Upsert(OrderUpsertRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Orders", request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<OrderUpsertResponse>();
            return result ?? new OrderUpsertResponse();
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<InvalidRequestResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new ArgumentException(result?.Message);
        }
        else
        {
            throw new Exception("Internal Server Error");
        }
    }
}
