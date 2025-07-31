namespace Dsw2025Tpi.Application.Dtos;

public record ProductModel
{
    public record Request(string Sku,string InternalCode, string Name, string Description, decimal CurrentUnitPrice, int StockQuantity, bool IsActive);
    public record PatchRequest( bool? IsActive);
    public record RequestP(string Sku, string InternalCode, string Name, string Description, decimal CurrentUnitPrice, int StockQuantity);

   

    public record Response(Guid Id);
}
