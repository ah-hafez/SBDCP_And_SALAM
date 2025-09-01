using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;
using MCS.DTO;

namespace MCS.UI.Areas.Admin.Models
{
    public class EditUserProfileVM
    {
        public int Id { get; set; }

        public int IdentifierId { get; set; }

        [CustomDisplayName("Admin.User.UserName")]
        [CustomRequired("Admin.User.UserNameRequired")]
        [CustomStringLength("Admin.User.UserNameLength", 50, 0)]
        public string UserName { get; set; }

        public bool IsActive { get; set; }

        [CustomDisplayName("Admin.User.TitleLevel")]
        [CustomRequired("Admin.User.TitleRequired")]
        public int TitleId { get; set; }
        public string TitleName { get; set; }

        [CustomDisplayName("Admin.User.EmployeeCategoryLevel")]
        [CustomRequired("Admin.User.EmployeeCategoryRequired")]
        public int CategoryId { get; set; }

        [CustomDisplayName("Admin.User.ProcessingTime")]
        [CustomRequired("Admin.User.ProcessingTimeRequired")]
        [CustomRangeAttribute("Admin.User.ProcessingTimeRange", 1, 500)]
        public int TransactionProcessingPeriod { get; set; }

        [CustomDisplayName("Admin.User.Departments")]
        public List<int> OrgUnits { get; set; }//OrgUnitDTO
        public List<OrgUnitDTO> OrgUnitList { get; set; }
        [CustomDisplayName("Admin.OrgUnitInfo.IsRoot")]
        [CustomRequired("Admin.OrgUnitInfo.UintRequired")]
        public int? MainOrgUnitId { get; set; }
        public string MainOrgUnitName { get; set; }

        [CustomDisplayName("Admin.User.Email")]
        [CustomEmailAddress("Admin.User.EmailSyntax")]
        [CustomStringLength("Admin.User.EmailLength", 50, 0)]
        public string Email { get; set; }

        [CustomDisplayName("Admin.User.PhoneNumber")]
        [CustomStringLength("Admin.User.PhoneNumberLength", 15, 0)]
        public string PhoneNumber { get; set; }

        public List<LocalizationVM> Names { get; set; }

        [CustomDisplayName("Admin.User.Permissions")]
        public List<int> Permissions { get; set; }

        [CustomRequired("Admin.User.UserNationalityIdRequired")]
        [CustomDisplayName("Admin.User.UserNationalityId")]
        [CustomStringLength("Admin.User.UserNationalityIdLength", 10, 0)]
        public string UserNationalId { get; set; }
        public bool IsManager { get; set; }
        [CustomRequired("Admin.User.RoleRequired")]
        [CustomDisplayName("Admin.User.Role")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        [CustomRequired("User.Transaction.Name.GenderRequired")]
        [CustomDisplayName("User.Transaction.Name.Gender")]      
        public int Gender { get; set; }

        public string SelectedOrgUnitsIds { get; set; }// OtherOrgUnitDTOs

        [CustomDisplayName("User.UserProfile.NewPassword")]
        [CustomRequired("User.UserProfile.NewPasswordRequierd")]
        [DataType(DataType.Password)]
        public string EditPassword { get; set; }

        [DataType(DataType.Password)]
        [CustomDisplayName("User.UserProfile.ReWritePassword")]
        [CustomRequired("User.UserProfile.ReNewPasswordRequierd")]
        [CustomCompare("EditPassword", "User.UserProfile.ReNewPasswordCompare")]
        public string ConfirmEditPassword { get; set; }

        public int? ExternalId { get; set; }

        public List<UserGroupDTO> UserGroups { get; set; }

        public string UserGroupsList { get; set; }

        public List<int> UserGroupsData { get; set; }
        public bool SMSNotifications { get; set; }
        public bool IsFollowUpUser { get; set; }
        public bool AllowMobile { get; set; }
        public string InternalNumber { get; set; }
        public string ApiKey { get; set; }
        [CustomDisplayName("Admin.User.UserMobileClass")]
        public int? UserMobileClassId { get; set; }
        public string UserMobileClassName { get; set; }

    }
}