using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Exceptions
{
    public class BusinessValidationException : BaseException
    {
       public BusinessValidationException() : base() { }

        public BusinessValidationException(string message) : base(message) { }

        public BusinessValidationException(string message, Exception inner) : base(message, inner) { }

        protected BusinessValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
