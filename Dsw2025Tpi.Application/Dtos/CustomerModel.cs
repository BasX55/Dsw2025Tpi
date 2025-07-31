using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dsw2025Tpi.Domain.Entities;

namespace Dsw2025Tpi.Application.Dtos
{
    public record CustomerModel
    {
        public record Request(string Email, string Name, int PhoneNumber);

        public record Response(Guid Id, string Name, string Email, string PhoneNumber);
    }
}
