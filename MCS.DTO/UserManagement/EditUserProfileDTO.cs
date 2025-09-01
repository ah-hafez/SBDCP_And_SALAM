using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class EditUserProfileDTO
    {
        public int Id { get; set; }

        public int IdentifierId { get; set; }

        //[CustomDisplayName("Admin.User.UserName")]
        [CustomRequired("Admin.User.UserNameRequired")]
        [CustomStringLength("Admin.User.UserNameLength", 50, 0)]
        public string UserName { get; set; }

        public bool IsActive { get; set; }

        //[CustomDisplayName("Admin.User.TitleLevel")]
        [CustomRequired("Admin.User.TitleRequired")]
        public int TitleId { get; set; }
        public string TitleName { get; set; }

        //[CustomDisplayName("Admin.User.EmployeeCategoryLevel")]
        [CustomRequired("Admin.User.EmployeeCategoryRequired")]
        public int CategoryId { get; set; }

        //[CustomDisplayName("Admin.User.ProcessingTime")]
        [CustomRequired("Admin.User.ProcessingTimeRequired")]
        [CustomStringLength("Admin.User.ProcessingTimeLength", 3, 1)]
        [CustomRegularExpression("^[1-9][0-9]*$", "Admin.User.ProcessingTimeExpression")]
        public int TransactionProcessingPeriod { get; set; }

        //[CustomDisplayName("Admin.User.Departments")]
        public List<int> OrgUnits { get; set; }//OrgUnitDTO

        //[CustomDisplayName("Admin.User.Email")]
        [CustomEmailAddress("Admin.User.EmailSyntax")]
        //[CustomRequired("Admin.User.EmailRequired")]
        [CustomStringLength("Admin.User.EmailLength", 50, 0)]
        public string Email { get; set; }
        public string MainOrgUnitName { get; set; }

        //[CustomDisplayName("Admin.User.PhoneNumber")]
        //[CustomRequired("Admin.User.PhoneNumberRequired")]
        [CustomStringLength("Admin.User.PhoneNumberLength", 12, 0)]
        public string PhoneNumber { get; set; }

        public List<LocalizationDTO> Names { get; set; }

        //[CustomDisplayName("Admin.User.Permissions")]
        public List<int> Permissions { get; set; }
        public List<int> Groups { get; set; }

        [CustomStringLength("Admin.User.UserNationalityIdLength", 10, 0)]
        public string UserNationalId { get; set; }
        public bool IsManager { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int Gender { get; set; }
        public int MainOrgUnitId { get; set; }
        public string Password { get; set; }

        public int? UserImageId { get; set; }
        public int? ExternalId { get; set; }

        public List<OrgUnitDTO> OrgUnitList { get; set; }
        public List<UserGroupDTO> UserGroups { get; set; }

        public List<int> UserGroupsData { get; set; }
        public bool SMSNotifications { get; set; }
        public bool IsFollowUpUser { get; set; }
        public bool AllowMobile { get; set; }
        public string InternalNumber { get; set; }
        public string ApiKey { get; set; }

        public int? UserMobileClassId { get; set; }
        public string UserMobileClassName { get; set; }
    }
}
