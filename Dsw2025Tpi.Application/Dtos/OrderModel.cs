using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderModel
    {
        public record Request(
        Guid CustomerId,
        string ShippingAddress,
        string BillingAddress,
        ICollection<OrderItem> OrderItems);
        public record OrderItem(
            Guid ProductId,
            int Quantity,
            string Name,
            decimal UnitPrice,
            string Description);
        public record Response(Guid Id);
    }
}
