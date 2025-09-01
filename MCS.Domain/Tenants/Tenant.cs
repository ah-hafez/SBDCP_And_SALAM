using System;
using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Tenant : EntityBase
    {
        public virtual TenantLocalizationIdentifier Name { get; set; }
        public string DatabaseName { get; set; }
        public string HostName { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDateH { get; set; }
        public DateTime ToDate { get; set; }
        public string ToDateH { get; set; }
        public int? OrgUnitsCount { get; set; }
        public int? UsersCount { get; set; }
        public virtual TenantLocalizationIdentifier DelegatedName { get; set; }
        public string DelegatedUserName { get; set; }
        public string DelegatedEmail { get; set; }
        public string DelegatedMobile { get; set; }
        public string LocalName { get; set; }
        public string LocalDelegatedName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public byte[] YesserCertificate { get; set; }
        public string YesserCode { get; set; }
        public string YesserSourceID { get; set; }
        public string YesserServiceID { get; set; }
        public string YesserSourceName { get; set; }
        public string SendingUsername { get; set; }
        public string SendingPassword { get; set; }
        public string RecievingUsername { get; set; }
        public string RecievingPassword { get; set; }
        public Byte[] Logo { get; set; }
        public string ECMProfileId { get; set; }
        public string ECMCategoryId { get; set; }
        public List<UserTenant> UserTenants { get; set; }
    }
}
