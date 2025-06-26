using Dsw2025Ej15.Application.Dtos;
using Dsw2025Ej15.Application.Services;
using Dsw2025Tpi.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Dsw2025Ej15.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductsManagementService _service;

    public ProductsController(ProductsManagementService service)
    {
        _service = service; 
    }

    [HttpGet()]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();
        if (products == null || !products.Any()) return NoContent();
        var result = products.Select(p => new
        {
            p.Sku,
            p.Name,
            p.Description,
            p.CurrentUnitPrice,
            p.StockQuantity,
            p.IsActive
        });
        return Ok(result);
    }

  
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductBySku(Guid id)
    {
        var product = await _service.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost()]
    public async Task<IActionResult> AddProduct([FromBody]ProductModel.Request request)
    {
        try
        {
            var product = await _service.AddProduct(request);
            return Ok(product);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch(DuplicatedEntityException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al guardar el producto");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.Request request)
    {
        try
        {
            var product = await _service.GetProductById(id);
            if (product == null) return NotFound();
            var product2 = await _service.UpdateProduct(id, request);
            return Ok(product2);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar el producto");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchProduct(Guid id, [FromBody] ProductModel.PatchRequest request)
    {
        try
        {
            var product = await _service.GetProductById(id);
            if (product == null) return NotFound();
            var prouctPatch = _service.PathProduct(id, request);
            return Ok(product);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar parcialmente el producto");
        }
    }
    /*
     * 6. Crear una nueva orden:
○ Método HTTP: POST
○ Ruta: /api/orders
○ Descripción: Permite registrar una nueva orden de compra en el sistema.
La orden debe incluir un identiers
     *//*
    [HttpPost()]
    public async Task<IActionResult> AddOrder() 
    {
        return 
    
    }*/


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            var product = await _service.GetProductById(id);
            if (product == null) return NotFound();
            await _service.DeleteProduct(id);
            return NoContent();
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al eliminar el producto");
        }
    }

}
