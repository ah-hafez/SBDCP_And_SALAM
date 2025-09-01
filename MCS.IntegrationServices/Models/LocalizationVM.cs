using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    [Serializable()]
    public class LocalizationVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        // [CustomRequired("Admin.UnitInfo.Names")]
        // [CustomStringLength("Global.Localization.Text", 100, 0)]
        // [CustomRegularExpression("^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", "Global.Localization.TextExpression")]
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("cultureId")]

        public int CultureId { get; set; }
        [JsonProperty("cultureName")]

        public string CultureName { get; set; }
    }
}