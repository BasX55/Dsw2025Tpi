namespace Dsw2025Tpi.Domain.Entities
{
    public class Product : EntityBase
    {
        public Product(string sku, string internalCode, string name, string description, decimal currentUnitPrice, int stockQuantity, bool isActive) : base()
        {
            InternalCode = internalCode;
            Sku = sku;
            Name = name;
            Description = description;
            CurrentUnitPrice = currentUnitPrice;
            StockQuantity = stockQuantity;
            IsActive = isActive;
        }

        public Product() { }
        public required string InternalCode { get; set; }
        public required string Sku { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal CurrentUnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }

       public void DecreaseStock(int quantity)
       {
            if (quantity <= 0) throw new ArgumentException("La cantidad a descontar debe ser mayor a cero");
            if (quantity > StockQuantity) throw new ArgumentException("No hay suficiente stock para descontar la cantidad solicitada");
            StockQuantity -= quantity;
       }
       

    }
}
