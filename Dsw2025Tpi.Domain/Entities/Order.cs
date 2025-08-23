namespace Dsw2025Tpi.Domain.Entities
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order : EntityBase
    {
        public Order() : base()
        {
        }
        public DateTime Date { get; set; }
        public required string ShippingAddress { get; set; }
        public required string BillingAddress { get; set; }
        public string? Notes { get; set; }
        public decimal TotalAmount => OrderItems?.Sum(item => item.Subtotal) ?? 0;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Guid CustomerID { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

    }
}
