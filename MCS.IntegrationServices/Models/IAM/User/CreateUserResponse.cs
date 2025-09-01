using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models.IAM.User
{
    public class CreateUserResponse : ApiBaseResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

    }
}