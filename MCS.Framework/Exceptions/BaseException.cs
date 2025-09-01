using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Exceptions
{
    [Serializable]
    public class BaseException : Exception
    {
        public BaseException() : base() { }

        public BaseException(string message) : base(message) { }

        public BaseException(string message, Exception inner) : base(message, inner) { } 
                    
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
