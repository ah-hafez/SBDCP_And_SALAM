using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models
{
    public class UserProfileVM
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string LocalName { get; set; }

        public string Category { get; set; }

        public string Email { get; set; }

        public List<LocalizationVM> Names { get; set; }
        [CustomDisplayName("Admin.User.Departments")]
        public List<int?> OrgUnits { get; set; }
        public bool IsSelected { get; set; }
        public string MainOrgUnitName { get; set; }

        public bool IsActive { get; set; }
        public bool IsManager { get; set; }
        public int RoleId { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public List<string> OrgUnitsNames { get; set; }

        public bool IsDeleted { get; set; }

        public int? ExternalId { get; set; }

        public List<int> UserGroups { get; set; }
        public bool AllowMobile { get; set; }
        public string InternalNumber { get; set; }
        public string ApiKey { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LastLogout { get; set; }


    }
}