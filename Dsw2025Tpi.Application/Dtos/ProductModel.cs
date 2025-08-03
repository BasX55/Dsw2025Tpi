namespace Dsw2025Tpi.Application.Dtos;

public record ProductModel
{
    
    public record PatchRequest( bool? IsActive);
    public record RequestP(string Sku, string InternalCode, string Name, string Description, decimal CurrentUnitPrice, int StockQuantity);

   

   
    public record ProductResponse(Guid Id, string Sku, string InternalCode, string Name, string Description,
        decimal CurrentUnitPrice, int StockQuantity, bool IsActive);
    public record GetResponse(string Sku, string InternalCode, string Name, string Description, decimal CurrentUnitPrice, int StockQuantity);
}
