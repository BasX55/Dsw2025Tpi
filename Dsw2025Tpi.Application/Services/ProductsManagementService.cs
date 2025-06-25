using Dsw2025Ej15.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System.Text.Json;

namespace Dsw2025Ej15.Application.Services;

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

    public async Task<ProductModel.Response> AddProduct(ProductModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Sku) || 
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Description) ||
            request.StockQuantity <= 0 ||
            request.CurrentUnitPrice < 0)
        {
            throw new ArgumentException("Valores para el producto no válidos");
        }
        var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
        //var exist = await _repository.First<Product>(p => p.Id == request.Sku);
        if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");

        var product = new Product(request.Sku, request.Name, request.Description, (decimal)request.CurrentUnitPrice, request.StockQuantity, request.IsActive);
        product.Id = Guid.NewGuid();
        var lista = await _repository.Add(product);
        // Convertir a JSON
        //string jsonProduct = JsonSerializer.Serialize(lista);


        // Guardarlo en un archivo (opcional)
        //await File.WriteAllTextAsync("C:\\Users\\moran\\OneDrive\\Desktop\\DSW2025\\tfi\\Dsw2025Tpi\\Dsw2025Tpi.Data\\Sources\\products.json", jsonProduct);

        return new ProductModel.Response(product.Id);
    }
}
