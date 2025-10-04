using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Dsw2025Tpi.Api.Controllers;



[ApiController]

[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly CustomerManagementService _service;
    public CustomersController(CustomerManagementService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _service.GetAllCustomers();
        return Ok(customers);
    }



    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("El ID del cliente no puede ser un Guid vacío");

        var customer = await _service.GetCustomerById(id);
        if (customer == null)
            return NotFound("Cliente no encontrado");

        return Ok(customer);
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerModel.Request request)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        
        var product = await _service.AddCustomer(request);
        return StatusCode(StatusCodes.Status201Created, product);
        
    }



}

