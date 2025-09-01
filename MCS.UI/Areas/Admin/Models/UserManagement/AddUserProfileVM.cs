using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;
using MCS.DTO;

namespace MCS.UI.Areas.Admin.Models
{
    public class AddUserProfileVM
    {
        public int Id { get; set; }

        [CustomDisplayName("Admin.User.UserName")]
        [CustomRequired("Admin.User.UserNameRequired")]
        [CustomStringLength("Admin.User.UserNameLength", 50, 0)]
        public string UserName { get; set; }

        public bool IsActive { get; set; }

        [CustomDisplayName("Admin.User.TitleLevel")]
        [CustomRequired("Admin.User.TitleRequired")]
        public int TitleId { get; set; }

        [CustomDisplayName("Admin.User.EmployeeCategoryLevel")]
        [CustomRequired("Admin.User.EmployeeCategoryRequired")]
        public int CategoryId { get; set; }

        [CustomDisplayName("Admin.User.ProcessingTime")]
        [CustomRequired("Admin.User.ProcessingTimeRequired")]
        [CustomRangeAttribute("Admin.User.ProcessingTimeRange", 1, 500)]
        //[CustomRegularExpression("^[1-9][0-9]*$", "Admin.User.ProcessingTimeExpression")]
        public int TransactionProcessingPeriod { get; set; }

        [CustomDisplayName("Admin.User.Departments")]
        public List<int> OrgUnits { get; set; }//OrgUnitDTO
        [CustomDisplayName("Admin.OrgUnitInfo.IsRoot")]
        [CustomRequired("Admin.OrgUnitInfo.UintRequired")]
        public int? MainOrgUnitId { get; set; }

        public List<OrgUnitDTO> OrgUnitList { get; set; }

        [CustomDisplayName("Admin.User.Email")]
        [CustomEmailAddress("Admin.User.EmailSyntax")]
        [CustomStringLength("Admin.User.EmailLength", 50, 0)]
        [CustomRequired("Admin.User.EmailRequired")]
        public string Email { get; set; }


        [CustomDisplayName("Admin.User.PhoneNumber")]
        //[CustomRequired("Admin.User.PhoneNumberRequired")]
        [CustomStringLength("Admin.User.PhoneNumberLength", 15, 0)]
        public string PhoneNumber { get; set; }
        [CustomRequired("Admin.User.NameReqd")]
        [CustomStringLength("Admin.User.NamesLength", 150, 0)]

        public List<LocalizationVM> Names { get; set; }

        [CustomDisplayName("Admin.User.Permissions")]
        public List<int> Permissions { get; set; }

        [CustomDisplayName("Admin.User.UserNationalityId")]
        //[CustomRequired("Admin.User.UserNationalityIdRequired")] 
        [CustomStringLength("Admin.User.UserNationalityIdLength", 10, 0)]
        public string UserNationalId { get; set; }

        public bool IsManager { get; set; }
        [CustomRequired("Admin.User.RoleRequired")]
        [CustomDisplayName("Admin.User.Role")]
        public int RoleId { get; set; }

        public string SelectedOrgUnitsIds { get; set; }// OtherOrgUnitDTOs

        [CustomDisplayName("User.Transaction.Name.Gender")]
        [CustomRequired("User.Transaction.Name.GenderRequired")]
        public int Gender { get; set; }

        [CustomDisplayName("User.UserProfile.NewPassword")]
        [CustomRequired("User.UserProfile.NewPasswordRequierd")]
        [DataType(DataType.Password)]
        [CustomStringLength("User.UserProfile.Length", 100, 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [CustomDisplayName("User.UserProfile.ReWritePassword")]
        [CustomRequired("User.UserProfile.ReNewPasswordRequierd")]
        [CustomCompare("Password", "User.UserProfile.ReNewPasswordCompare")]
        [CustomStringLength("User.UserProfile.Length", 100, 6)]
        public string ConfirmPassword { get; set; }
        public string UserGroups { get; set; }

        public List<int> UserGroupsList { get; set; }
        public bool SMSNotifications { get; set; }
        public bool IsFollowUpUser { get; set; }

        public bool? PendingRegestration { get; set; }
        public bool AllowMobile { get; set; }
        public string InternalNumber { get; set; }

        public string ApiKey { get; set; }
        [CustomDisplayName("Admin.User.UserMobileClass")]
        public int? UserMobileClassId { get; set; }
        public string UserMobileClassName { get; set; }

    }
}