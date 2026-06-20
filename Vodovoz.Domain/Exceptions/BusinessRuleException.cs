using System;
using System.Collections.Generic;
using System.Text;

namespace Vodovoz.Domain.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }

        public BusinessRuleException(string message, Exception inner) : base(message, inner) { }
    }
}
