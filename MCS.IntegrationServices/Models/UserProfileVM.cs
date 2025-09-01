using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class UserProfileVM
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string LocalName { get; set; }

        public string Category { get; set; }

        public string Email { get; set; }

        //public List<LocalizationVM> Names { get; set; }
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

    }
}