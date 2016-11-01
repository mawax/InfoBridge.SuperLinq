using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Exceptions
{
    public class SuperLinqException : Exception
    {
        public SuperLinqException() : base() { }
        public SuperLinqException(string message) : base(message) { }
        public SuperLinqException(string message, Exception innerException) : base(message, innerException) { }
    }
}
