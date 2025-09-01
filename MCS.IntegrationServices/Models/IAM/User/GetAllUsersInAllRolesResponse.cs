using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class GetAllUsersInAllRolesResponse : GetAllBaseResponse
    {
        [JsonProperty("roles")]
        public List<UserRoleModel> Roles { get; set; }

    }

    public class UserRoleModel
    {

        [JsonProperty("roleId")]
        public int RoleId { get; set; }

        [JsonProperty("roleName")]
        public string RoleName { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("users")]
        public List<BasicUserResponse> Users { get; set; }
    }

    public class BasicUserResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}