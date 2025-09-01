using MCS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class GetAllRoleResponse : GetAllBaseResponse
    {
        [JsonProperty("roles")]
        public List<RoleModel> Roles { get; set; }

    }

    public class RoleModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("permissions")]
        public List<PermissionModel> Permissions { get; set; }


    }



    public class PermissionModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

    }


}