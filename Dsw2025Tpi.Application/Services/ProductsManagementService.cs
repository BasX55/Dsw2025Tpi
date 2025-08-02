using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;


namespace Dsw2025Tpi.Application.Services;

public class ProductsManagementService
{
    private readonly IRepository _repository;

    public ProductsManagementService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        var product = await _repository.GetById<Product>(id);
        return product;
    }
    public async Task<List<Product>?> GetProducts() => (List<Product>?)await _repository.GetAll<Product>();

    public async Task<ProductModel.Response> AddProduct(ProductModel.RequestP request)
    {
       if (string.IsNullOrWhiteSpace(request.Sku) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.InternalCode)||
            string.IsNullOrWhiteSpace(request.Description) ||
            request.StockQuantity < 0 ||
            request.CurrentUnitPrice <= 0)   
        {
            throw new ArgumentException("Valores para el producto no válidos");
        }
        var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
        
        if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");

        var product = new Product(request.Sku, request.InternalCode, request.Name, request.Description, (decimal)request.CurrentUnitPrice, request.StockQuantity, true);
        product.Id = Guid.NewGuid();
        await _repository.Add(product);

        return new ProductModel.Response(product.Id);
    }

  
    public async Task<ProductModel.Response> PutProduct(Guid id, ProductModel.RequestP request)
    {
        var product = await _repository.GetById<Product>(id);
        if (product == null) throw new ArgumentException($"No existe un producto con el Id {id}");
        product.Sku = request.Sku;
        product.Name = request.Name;
        product.InternalCode = request.InternalCode;
        product.Description = request.Description;
        product.CurrentUnitPrice = request.CurrentUnitPrice;
        product.StockQuantity = request.StockQuantity;
        
        await _repository.Update(product);

        return new ProductModel.Response(product.Id);
    }

    public async Task<ProductModel.Response> PathProduct(Guid id, ProductModel.PatchRequest request)
    {
        var product = await _repository.GetById<Product>(id);
        if (product == null) throw new ArgumentException($"No existe un producto con el Id {id}");

        if (request.IsActive.HasValue) product.IsActive = request.IsActive.Value;

        await _repository.Update(product);
        return new ProductModel.Response(product.Id);
    }
    public async Task<ProductModel.Response> DeleteProduct(Guid id)
    {
        var product = await _repository.GetById<Product>(id);
        if (product == null) throw new ArgumentException($"No existe un producto con el Id {id}");
        var result = await _repository.Delete(product);
        await _repository.Update(result);
        return new ProductModel.Response(product.Id);
    }


}
