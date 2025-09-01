using System;

namespace MCS.UI.Areas.User.Models.Tenants
{
    public class TenantVM
    {
        public int Id { get; set; }
        public string LocalName { get; set; }
        public string FromDateH { get; set; }
        public string ToDateH { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? OrgUnitsCount { get; set; }
        public int? UsersCount { get; set; }
        public string LocalDelegatedName { get; set; }
        public string DelegatedUserName { get; set; }
        public string DelegatedMobile { get; set; }
        public string DelegatedEmail { get; set; }
        public string HostName { get; set; }
        public bool IsActive { get; set; }
    }
}