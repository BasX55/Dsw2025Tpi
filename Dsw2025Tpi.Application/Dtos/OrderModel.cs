namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderModel
    {
        public record Request(
         Guid CustomerId,
         string ShippingAddress,
         string BillingAddress,
         string Notes,
         ICollection<OrderItem> OrderItems);

        public record OrderItem(
            Guid ProductId,
            int Quantity);

        public record Response(
            Guid Id,
            DateTime Date,
            Guid CustomerId,
            string ShippingAddress,
            string BillingAddress,
            string Notes,
            string Status,
            decimal TotalAmount,
            ICollection<OrderItemResponse> OrderItems);

        public record OrderItemResponse(
            Guid ProductId,
            int Quantity,
            decimal UnitPrice,
            string Description);
        public record ResponseId(
           Guid Id);

        public record UpdateStatusRequest(string NewStatus);

    }


}
