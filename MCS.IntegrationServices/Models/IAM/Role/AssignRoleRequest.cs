using MCS.IntegrationServices.Models.IAM.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models.IAM.Role
{
    public class AssignRoleRequest : BaseRequest
    {
        [JsonProperty("roleId")]
        [Required]
        public int RoleId { get; set; }
        [JsonProperty("userId")]
        [Required]
        public int UserId { get; set; }

    }
}