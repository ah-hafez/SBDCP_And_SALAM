using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class OrgUnitUserVM
    {
        [CustomRequired("Admin.OrgUnitUsers.UserName")]
        [CustomDisplayName("Admin.OrgUnitUsers.UserName")]
        public int Id { get; set; }

        public string UserName { get; set; }


        public string LocalName { get; set; }

        public string Category { get; set; }

        public string Email { get; set; }

       
        public bool IsSelected { get; set; }
        public string MainOrgUnitName { get; set; }

        public bool IsActive { get; set; }
        public bool IsManager { get; set; }
        public int RoleId { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public int? ExternalId { get; set; }


    }
}