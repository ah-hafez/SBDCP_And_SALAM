using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.Tenants.Service.Models
{
    public class TokenData
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("userName")]
        public string Username { get; set; }
        [JsonProperty("userIdentity")]
        public string UserIdentity { get; set; }
        [JsonProperty(".issued")]
        public string IssuedAt { get; set; }
        [JsonProperty(".expires")]
        public string ExpiresAt { get; set; }
    }
}