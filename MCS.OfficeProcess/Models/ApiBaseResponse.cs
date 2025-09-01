using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.OfficeProcess.Models
{
    public class ApiBaseResponse
    {
        public ApiBaseResponse()
        {
            ResponseMessage = "";
        }
        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }
        [JsonProperty("responseMessage")]

        public string ResponseMessage { get; set; }

    }
}