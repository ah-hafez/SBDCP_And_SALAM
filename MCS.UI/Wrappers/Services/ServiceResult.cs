using System.Collections.Generic;

namespace MCS.UI.Wrappers
{
    public class ServiceResult<TResult>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, object> ModelErrorMessages { get; set; }
        public Dictionary<string, object> ErrorMessages { get; set; }
        public TResult Result { get; set; }
    }
}