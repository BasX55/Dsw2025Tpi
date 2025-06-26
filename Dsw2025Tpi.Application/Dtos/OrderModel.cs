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
        public record Request(DateTime Date, string ShippingAddress, string BillingAddress, string Notes, decimal TotalAmount, OrderStatus status, Customer customer,ICollection<OrderItem> orderItem);
        
        public record Response(Guid Id);
    }
}
