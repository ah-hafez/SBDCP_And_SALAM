using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.TenantsAdmin.Models.Tenant
{
    public class UserTenantVM
    {
        public int? Id { get; set; }
        [CustomDisplayName("Tenant.Name")]
        public int TenantId { get; set; }
        [CustomDisplayName("Tenant.Name")]
        public string LocalName { get; set; }
        [CustomDisplayName("Tenant.Login.UserName")]
        public string UserName { get; set; }
        public Mode Mode { get; set; }
    }
}