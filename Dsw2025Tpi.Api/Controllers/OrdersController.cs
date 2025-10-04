using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;
[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly OrderManagementService _service;

    public OrderController(OrderManagementService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "CLIENTE")]
    public async Task<IActionResult> AddOrder([FromBody] OrderModel.Request request)
    {
        var order = await _service.AddOrder(request);
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);

    }


    [HttpGet("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var order = await _service.GetOrderById(id);
        return Ok(order);
       
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR, CLIENTE")]
    public async Task<IActionResult> GetAllOrders(
    [FromQuery] string? status,
    [FromQuery] Guid? customerId,
    [FromQuery] int? pageNumber,
    [FromQuery] int? pageSize)
    {        
        var orders = await _service.GetAllOrders(status, customerId, pageNumber, pageSize);
        return Ok(orders);        
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderModel.UpdateStatusRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedOrder = await _service.UpdateOrderStatus(id, request.NewStatus);
        return Ok(updatedOrder);
    }

}

