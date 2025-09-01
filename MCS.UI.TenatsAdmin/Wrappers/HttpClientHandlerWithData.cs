using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MCS.UI.TenantsAdmin.Wrappers
{
    public class HttpClientHandlerWithData : HttpClientHandler
    {
        protected Dictionary<string, object> Data { get; set; }

        public HttpClientHandlerWithData(Dictionary<string, object> data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}