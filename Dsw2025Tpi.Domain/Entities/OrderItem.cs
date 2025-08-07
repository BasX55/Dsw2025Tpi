namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public OrderItem() : base()
        {

        }

        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        // Propiedad calculada para el subtotal del item de la orden

        public decimal Subtotal => Quantity * UnitPrice;
        // Navigation properties
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
