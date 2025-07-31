using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Dsw2025Tpi.Application.Dtos.OrderModel;

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
           
            var order = await _service.AddOrder(request);
            return Ok(order);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (DuplicatedEntityException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al guardar la orden");
        }
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("El ID de la orden no puede ser un Guid vacío");
        try
        {
            var order = await _service.GetOrderById(id);
            if (order == null)
                return NotFound("Orden no encontrada");
            return Ok(order);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al obtener la orden");
        }
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR, CLIENTE")]
    public async Task<IActionResult> GetAllOrders(string? status,Guid? customerId,int? pageNumber,int? pageSize)
    {
        try
        {
            var orders = await _service.GetAllOrders(status,customerId,pageNumber,pageSize);
            return Ok(orders);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al obtener las órdenes");
        }
    }
    [HttpPut("{id}/status")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        if (id == Guid.Empty)
            return BadRequest("El ID de la orden no puede ser un Guid vacío");
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var updatedOrder = await _service.UpdateOrderStatus(id, request.NewStatus);
            if (updatedOrder == null)
                return NotFound("Orden no encontrada");
            return Ok(updatedOrder);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar el estado de la orden");
        }
    }
}
