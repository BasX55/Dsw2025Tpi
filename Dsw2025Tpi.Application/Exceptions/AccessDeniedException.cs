using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
    public class AccessDeniedException : Exception
    {
        //excepcion para cuando el cliente intenta hacer operaciones de administrador
        public AccessDeniedException(string message) : base(message)
        {
        }
        public AccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
