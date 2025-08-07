using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.Extensions.Logging;


namespace Dsw2025Tpi.Application.Services;

public class ProductsManagementService
{
    private readonly IRepository _repository;
    private readonly ILogger<ProductsManagementService> _logger;

    public ProductsManagementService(IRepository repository, ILogger<ProductsManagementService> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<ProductModel.ProductResponse> GetProductById(Guid id)
    {
        _logger.LogInformation($"Obteniendo producto con ID: {id}");
        if (id == Guid.Empty)
            throw new ArgumentException("El ID del producto no puede ser un Guid vacío");

        var product = await _repository.GetById<Product>(id) ?? 
            throw new EntityNotFoundException($"No existe un producto con el Id {id}");


        return new ProductModel.ProductResponse(
            product.Id,
            product.Sku,
            product.InternalCode,
            product.Name,
            product.Description,
            product.CurrentUnitPrice,
            product.StockQuantity,
            product.IsActive
        );
    }
    public async Task<List<ProductModel.GetResponse>> GetProducts()
    {
        _logger.LogInformation("Obteniendo todos los productos activos");
        var products = await _repository.GetFiltered<Product>(p => p.IsActive);

        if (products == null || !products.Any())
            return new List<ProductModel.GetResponse>(); // No lanzar excepción

        return products.Select(p => new ProductModel.GetResponse(
            p.Sku,
            p.InternalCode,
            p.Name,
            p.Description,
            p.CurrentUnitPrice,
            p.StockQuantity)
        ).ToList();
    }

    public async Task<ProductModel.ProductResponse> AddProduct(ProductModel.RequestP request)
    {
        _logger.LogInformation("Agregando un nuevo producto");
        if (string.IsNullOrWhiteSpace(request.Sku))
            throw new ArgumentException("El campo Sku no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("El campo Name no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(request.InternalCode))
            throw new ArgumentException("El campo InternalCode no puede estar vacío.");

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new ArgumentException("El campo Description no puede estar vacío.");

        if (request.StockQuantity < 0)
            throw new ArgumentException("StockQuantity no puede ser menor a cero.");

        if (request.CurrentUnitPrice <= 0)
            throw new ArgumentException("CurrentUnitPrice debe ser mayor a cero.");

        var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
        
        if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");

        var product = new Product(request.Sku, request.InternalCode, request.Name, request.Description, (decimal)request.CurrentUnitPrice, request.StockQuantity, true);
        product.Id = Guid.NewGuid();
        await _repository.Add(product);
        _logger.LogInformation($"Producto agregado con ID: {product.Id}");

        return new ProductModel.ProductResponse(
            product.Id,
            product.Sku,
            product.InternalCode,
            product.Name,
            product.Description,
            product.CurrentUnitPrice,
            product.StockQuantity,
            product.IsActive
        );
    }

  
    public async Task<ProductModel.ProductResponse> PutProduct(Guid id, ProductModel.RequestP request)
    {
        _logger.LogInformation($"Actualizando producto con ID: {id}");
        var product = await _repository.GetById<Product>(id) ?? 
            throw new EntityNotFoundException($"No existe un producto con el Id {id}");
        if (string.IsNullOrWhiteSpace(request.Sku))
            throw new ArgumentException("El campo Sku no puede estar vacío.");

        var exist = await _repository.First<Product>(p => p.Sku == request.Sku && p.Id != id);

        if (exist != null) 
            throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("El campo Name no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(request.InternalCode))
            throw new ArgumentException("El campo InternalCode no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(request.Description))
            throw new ArgumentException("El campo Description no puede estar vacío.");
        if (request.StockQuantity < 0)
            throw new ArgumentException("StockQuantity no puede ser menor a cero.");
        if (request.CurrentUnitPrice <= 0)
            throw new ArgumentException("CurrentUnitPrice debe ser mayor a cero.");


        product.Sku = request.Sku;
        product.Name = request.Name;
        product.InternalCode = request.InternalCode;
        product.Description = request.Description;
        product.CurrentUnitPrice = request.CurrentUnitPrice;
        product.StockQuantity = request.StockQuantity;
        
        await _repository.Update(product);
        _logger.LogInformation($"Producto actualizado con ID: {product.Id}");

        return new ProductModel.ProductResponse(
            product.Id,
            product.Sku,
            product.InternalCode,
            product.Name,
            product.Description,
            product.CurrentUnitPrice,
            product.StockQuantity,
            product.IsActive
        );
    }


    public async Task PathProduct(Guid id, ProductModel.PatchRequest request)
    {
        _logger.LogInformation($"Actualizando estado del producto con ID: {id}");
        var product = await _repository.GetById<Product>(id)
            ?? throw new EntityNotFoundException($"No existe un producto con el Id {id}");

        if (request.IsActive.HasValue)
            product.IsActive = request.IsActive.Value;

        await _repository.Update(product);
        _logger.LogInformation($"Producto actualizado con ID: {product.Id}");
    }
    public async Task<ProductModel.ProductResponse> DeleteProduct(Guid id)
    {
        _logger.LogInformation($"Eliminando producto con ID: {id}");
        var product = await _repository.GetById<Product>(id) ?? 
            throw new EntityNotFoundException($"No existe un producto con el Id {id}");

        await _repository.Delete(product);

        _logger.LogInformation($"Producto eliminado con ID: {product.Id}");
        return new ProductModel.ProductResponse(
            product.Id,
            product.Sku,
            product.InternalCode,
            product.Name,
            product.Description,
            product.CurrentUnitPrice,
            product.StockQuantity,
            product.IsActive
        );
    }


}
