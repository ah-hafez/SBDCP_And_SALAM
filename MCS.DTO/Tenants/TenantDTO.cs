using System;
using MCS.Framework.MultiTenants;
using MCS.DTO.Tenants;

namespace MCS.DTO
{
    public class TenantDTO : BaseDTO, ITenant
    {
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
        public string DatabaseName { get; set; }
        public byte[] Logo { get; set; }
        public byte[] YesserCertificate { get; set; }
        public bool IsDeleted { get; set; }
        public TenantLocalizationIdentifierDTO Name { get; set; } = new TenantLocalizationIdentifierDTO();
        public TenantLocalizationIdentifierDTO DelegatedName { get; set; } = new TenantLocalizationIdentifierDTO();
    }
}
