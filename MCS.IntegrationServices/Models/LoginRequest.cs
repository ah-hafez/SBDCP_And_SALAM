using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class LoginRequest
    {
        [Required]
        [JsonProperty("userName")]
        public string UserName { get; set; }

        //[Required]
        ////[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [JsonProperty("password")]
        public string Password { get; set; }


    }
}