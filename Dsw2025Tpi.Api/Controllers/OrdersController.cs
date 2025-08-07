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
        try
        {
            var order = await _service.AddOrder(request);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        catch (ArgumentNullException ane)
        {
            return BadRequest(ane.Message);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (EntityNotFoundException enf)
        {
            return NotFound(enf.Message);
        }
        catch (InsufficientStockException ise)
        {
            return UnprocessableEntity(ise.Message); 
        }
        catch (InactiveProductException ipa)
        {
            return UnprocessableEntity(ipa.Message); 
        }
        catch (InvalidProductPriceException ippe)
        {
            return UnprocessableEntity(ippe.Message); 
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
        try
        {
            var order = await _service.GetOrderById(id);
            return Ok(order);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (EntityNotFoundException enf)
        {
            return NotFound(enf.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al obtener la orden");
        }
    }

    [HttpGet]
    [Authorize(Roles = "ADMINISTRADOR, CLIENTE")]
    public async Task<IActionResult> GetAllOrders(
    [FromQuery] string? status,
    [FromQuery] Guid? customerId,
    [FromQuery] int? pageNumber,
    [FromQuery] int? pageSize)
    {
        try
        {
            var orders = await _service.GetAllOrders(status, customerId, pageNumber, pageSize);
            return Ok(orders);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (EntityNotFoundException enf)
        {
            return NotFound(enf.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al obtener las órdenes.");
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderModel.UpdateStatusRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedOrder = await _service.UpdateOrderStatus(id, request.NewStatus);
            return Ok(updatedOrder);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (EntityNotFoundException enf)
        {
            return NotFound(enf.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar el estado de la orden.");
        }
    }
}
