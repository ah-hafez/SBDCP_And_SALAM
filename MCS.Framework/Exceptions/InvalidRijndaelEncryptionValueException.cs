using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Exceptions
{
    public class InvalidRijndaelEncryptionValueException : BaseException
    {
        public InvalidRijndaelEncryptionValueException() : base() { }

        public InvalidRijndaelEncryptionValueException(string message) : base(message) { }

        public InvalidRijndaelEncryptionValueException(string message, Exception inner) : base(message, inner) { }

        protected InvalidRijndaelEncryptionValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
