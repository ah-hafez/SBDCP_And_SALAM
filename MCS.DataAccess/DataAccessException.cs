using System;
using System.Runtime.Serialization;
using MCS.Framework.Exceptions;

namespace MCS.DataAccess
{
    public class DataAccessException : BaseException
    {
        #region Constructors

        public DataAccessException() { }

        public DataAccessException(string message)
            : base(message) { }

        public DataAccessException(string message, Exception innerException)
            : base(message, innerException) { }

        protected DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion Constructors

        #region Methods

        public static Exception Translate(Exception ex)
        {
            ExceptionHelper.HandleException(ex);

            return new DataAccessException("Data access operation failed!", ex);
        }

        #endregion Methods
    }
}
