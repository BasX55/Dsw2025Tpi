using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderItemModel
    {
        public record RequestS(int Quallity, decimal UnitPrice, Order Order, Product Product);

        public record Request();
        public record Response(Guid Id);
    }
}
