using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.TenantsAdmin.Models.LookupsVM;

namespace MCS.UI.TenantsAdmin.Models.Tenant
{
    public class AddTenantVM
    {
        public List<LocalizationVM> Names { get; set; }
        public string FromDateH { get; set; }
        public string ToDateH { get; set; }


        public DateTime FromDate { get; set; }


        public DateTime ToDate { get; set; }


        [CustomDisplayName("Tenant.OrgUnitsCount")]
        public int? OrgUnitsCount { get; set; }

        [CustomDisplayName("Tenant.UsersCount")]
        public int? UsersCount { get; set; }
        public List<LocalizationVM> DelegatedName { get; set; }

        [CustomDisplayName("Tenant.DelegatedUserName")]
        public string DelegatedUserName { get; set; }

        [CustomDisplayName("Tenant.DelegatedMobile")]
        public string DelegatedMobile { get; set; }
        [CustomDisplayName("Tenant.DelegatedEmail")]
        public string DelegatedEmail { get; set; }


        //[RegularExpression(@"([a-zA-Z\d]+[\w\d.]*|)[a-zA-Z]+[\w\d.]*", ErrorMessage = "Tanant.HostNameExpresssion")]
        [CustomDisplayName("Tenant.HostName")]
        public string HostName { get; set; }

        [CustomDisplayName("User.Tenant.Logo")]
        public byte[] Logo { get; set; }

        [CustomDisplayName("User.Tenant.YesserCertificate")]
        public byte[] YesserCertificate { get; set; }
    }
}