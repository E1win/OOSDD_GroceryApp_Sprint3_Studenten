using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Exceptions
{
    public class UsedEmailException : Exception
    {
        public UsedEmailException() : base() { }
        public UsedEmailException(string message) : base(message) { }
        public UsedEmailException(string message, Exception inner) : base(message, inner) { }
    }
}
