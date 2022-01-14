using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Exceptions
{
    public class OrderException:Exception
    {
        public OrderException() { }
        public OrderException(string message) : base(message) { }
        public OrderException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
