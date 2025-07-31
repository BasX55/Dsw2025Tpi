using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Authorize(Roles = "ADMINISTRADOR")]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductsManagementService _service;

    public ProductsController(ProductsManagementService service)
    {
        _service = service;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();
        if (products == null || !products.Any()) return NoContent();
        var result = products
    .Where(p => p.IsActive)
    .Select(p => new
    {
        p.Sku,
        p.Name,
        p.InternalCode,
        p.Description,
        p.CurrentUnitPrice,
        p.StockQuantity
    })
    .ToList();

        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _service.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductModel.RequestP request)
    {
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var product = await _service.AddProduct(request);
            return StatusCode(StatusCodes.Status201Created, product);
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
            return Problem("Se produjo un error al guardar el producto");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.RequestP request)
    {
        try
        {
            var product = await _service.GetProductById(id);
            if (product == null) return NotFound();
            var updatedProduct = await _service.PutProduct(id, request);
            return Ok(updatedProduct);

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
            await _service.PathProduct(id, request);
            return NoContent();
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar parcialmente el producto");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            var product = await _service.GetProductById(id);
            if (product == null) return NotFound();
            await _service.DeleteProduct(id);
            return Ok(product);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al eliminar el producto");
        }
    }

}

