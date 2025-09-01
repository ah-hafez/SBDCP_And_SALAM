using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class GetAllBaseResponse : ApiBaseResponse
    {
        [JsonProperty("totalRecord")]
        public int TotalRecord { get; set; }
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }
}