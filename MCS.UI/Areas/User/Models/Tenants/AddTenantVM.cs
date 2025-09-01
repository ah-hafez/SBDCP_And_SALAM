using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Tenants
{
    public class AddTenantVM
    {
        public List<LocalizationVM> Names { get; set; }
        public string FromDateH { get; set; }
        public string ToDateH { get; set; }

        [CustomDateTimeCompareAttribute("ToDate", Operation.LessThan, "Tenant.ToDateCompare")]
        [CustomRequired("Tenant.FromDateRequired")]
        public DateTime FromDate { get; set; }

        [CustomRequired("Tenant.ToDateRequired")]
        public DateTime ToDate { get; set; }

        [CustomDisplayName("Tenant.OrgUnitsCount")]
        [CustomStringLength("Tenant.OrgUnitsCountLength", 4)]
        public int? OrgUnitsCount { get; set; }

        [CustomDisplayName("Tenant.UsersCount")]
        [CustomStringLength("Tenant.UsersCountLength", 4)]
        public int? UsersCount { get; set; }
        public List<LocalizationVM> DelegatedName { get; set; }

        [CustomDisplayName("Tenant.DelegatedUserName")]
        [CustomRequired("Tenant.DelegatedUserNameRequired")]
        [CustomStringLength("Tenant.DelegatedUserNameLength", 50)]
        public string DelegatedUserName { get; set; }

        [CustomDisplayName("Tenant.DelegatedMobile")]
        [CustomRequired("Tenant.DelegatedMobileRequired")]
        [CustomStringLength("Tenant.DelegatedMobileLength", 20)]
        public string DelegatedMobile { get; set; }

        [CustomDisplayName("Tenant.DelegatedEmail")]
        [CustomRequired("Tenant.DelegatedEmailRequired")]
        [CustomEmailAddress("Tanant.DelegatedEmailExpresssion")]
        [CustomStringLength("Tenant.DelegatedEmailLength", 50)]
        [CustomRegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "Tanant.DelegatedEmailExpresssion")]
        public string DelegatedEmail { get; set; }

        [CustomDisplayName("Tenant.HostName")]
        [CustomRequired("Tenant.HostNameRequired")]
        [CustomStringLength("Tenant.HostNameLength", 100)]
        [CustomRegularExpression(@"^[a-zA-Z\d\.]+$", "Tenant.HostNameExpresssion")]
        //[RegularExpression(@"([a-zA-Z\d]+[\w\d.]*|)[a-zA-Z]+[\w\d.]*", ErrorMessage = "Tanant.HostNameExpresssion")]
        public string HostName { get; set; }
    }
}