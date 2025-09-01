using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models.IAM.Common
{
    public class BaseRequest
    {
        [JsonProperty("requestDate")]
        public string RequestDate { get; set; }
    }
}