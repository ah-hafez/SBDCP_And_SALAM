using MCS.IntegrationServices.Models.IAM.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class GetAllBaseRequest : BaseRequest
    {
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }
      

    }
}