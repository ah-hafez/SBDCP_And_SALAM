using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using MCS.Common;

namespace MCS.UI.TenantsAdmin.Common
{
    public class SerivceException: Exception
    {
        public StatusCode StatusCode { get; set; } = StatusCode.InternalServerError;
        public SerivceException(string message, StatusCode status) : base(message)
        {
            this.StatusCode = status;
        }
        public SerivceException(string message) : base(message)
        {
        }
        public SerivceException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected SerivceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}