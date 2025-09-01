using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class LocalizationRequest
    {

        [Required]
        [StringLength(100)]
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("cultureId")]
        public int CultureId { get; set; }


    }
}