using System.Collections.Generic;
using System.Net;

namespace MCS.Tenants.Service.Results
{
    public class JsonResponseResult : JsonResponseResult<string>
    {
    }

    public class JsonResponseResult<T>
    {
        public int StatusCode { get; set; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage) && 200 <= StatusCode && StatusCode < 300;

        public string ErrorMessage { get; set; }

        public Dictionary<string, object> ModelErrorMessages { get; set; }
        public Dictionary<string, object> ErrorMessages { get; set; }
        public T Result { get; set; }
        public JsonResponseResult()
        {

        }

        public JsonResponseResult(T result = default(T), string errorMessage = null, Dictionary<string, object> errorMessages = null, Dictionary<string, object> modelErrorMessages = null, int statusCode = (int)HttpStatusCode.OK)
        {
            StatusCode = statusCode;
            Result = result;
            ErrorMessage = errorMessage;
            ModelErrorMessages = modelErrorMessages;
            ErrorMessages = errorMessages;
        }
    }
}