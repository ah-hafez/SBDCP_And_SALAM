using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class TransactionBaseRequest : GetAllBaseRequest
    {

        [JsonProperty("userName")]
        public string UserName { get; set; }

    }
}