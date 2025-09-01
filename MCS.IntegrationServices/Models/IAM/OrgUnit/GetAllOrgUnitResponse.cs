using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class GetAllOrgUnitResponse : ApiBaseResponse
    {

        [JsonProperty("orgunits")]
        public IList<OrgUnitModel> Orgunits { get; set; }
    }

    public class OrgUnitModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}