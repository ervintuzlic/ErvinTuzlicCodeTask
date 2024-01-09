using IntusCodeTaskErvinTuzlic.Server.Services.Orders;
using IntusCodeTaskErvinTuzlic.Shared.DomainModel;
using IntusCodeTaskErvinTuzlic.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IntusCodeTaskErvinTuzlic.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    public readonly IOrdersService _ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Order>>> GetAll()
    {
        var orders = await _ordersService.GetAll();

        return Ok(orders);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Order>> Get(int id)
    {
        var order = await _ordersService.Get(id);

        if(order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<OrderUpsertResponse>> Upsert(OrderUpsertRequest request)
    {
        var result = await _ordersService.Upsert(request);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _ordersService.Delete(id);

        return NoContent();
    }
}
