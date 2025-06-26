using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Dtos
{
    public record CostumerModel
    {
        public record Request(string Email, string Name, int PhoneNumber, ICollection<Order> Orders);

        public record Response(Guid Id);
    }
}
