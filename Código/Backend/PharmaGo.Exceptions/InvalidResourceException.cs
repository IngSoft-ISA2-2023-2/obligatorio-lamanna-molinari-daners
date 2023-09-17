using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Exceptions
{
    public class InvalidResourceException: Exception
    {
        public InvalidResourceException(string message) : base(message)
        {
        }
    }
}
