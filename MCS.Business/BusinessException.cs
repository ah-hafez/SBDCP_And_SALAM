using System;
using System.Runtime.Serialization;
using MCS.Framework.Exceptions;
using MCS.Common;

namespace MCS.Business
{
    public class BusinessException : BaseException
    {
        #region Constructors

        public BusinessException() { }

        public BusinessException(StatusCode statusCode)
            : base(statusCode.ToString()) { }

        public BusinessException(string message)
            : base(message) { }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException) { }

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion Constructors

        #region Methods

        public static Exception Translate(Exception ex)
        {
            ExceptionHelper.HandleException(ex);

            return new BusinessException(StatusCode.GeneralError.ToString(), ex);
        }

        #endregion Methods
    }
}
