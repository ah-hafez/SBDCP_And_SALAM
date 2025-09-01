using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Exceptions;

namespace MCS.Framework.Notifications
{
    public class NotificationConfigurationException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the NotificationServiceConfigurationException class.
        /// </summary>
        public NotificationConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotificationServiceConfigurationException class.
        /// </summary>
        /// <param name="message">An object of type string contain exception message.</param>
        public NotificationConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotificationServiceConfigurationException class.
        /// </summary>
        /// <param name="message">An object of type string contain exception message.</param>
        /// <param name="exception">An object of type Exception .</param>
        public NotificationConfigurationException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotificationServiceConfigurationException class.
        /// </summary>
        /// <param name="info"> The System.Runtime.Serialization.SerializationInfo that holds the serialized
        ///  object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual
        /// information about the source or destination.</param>
        protected NotificationConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
