using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.UserManagement
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

        [CustomDisplayName("Admin.User.EmployeeCategoryLevel")]
        [CustomRequired("Admin.User.EmployeeCategoryRequired")]
        public int CategoryId { get; set; }

        [CustomDisplayName("Admin.User.ProcessingTime")]
        [CustomRequired("Admin.User.ProcessingTimeRequired")]
        [CustomStringLength("Admin.User.ProcessingTimeLength", 3, 1)]
        [CustomRegularExpression("^[1-9][0-9]*$", "Admin.User.ProcessingTimeExpression")]
        public int TransactionProcessingPeriod { get; set; }

        [CustomDisplayName("Admin.User.Departments")]
        public List<int> OrgUnits { get; set; }//OrgUnitDTO

        [CustomDisplayName("Admin.User.Email")]
        [CustomEmailAddress("Admin.User.EmailSyntax")]
        [CustomRequired("Admin.User.EmailRequired")]
        [CustomStringLength("Admin.User.EmailLength", 50, 0)]
        public string Email { get; set; }

        [CustomDisplayName("Admin.User.PhoneNumber")]
        [CustomRequired("Admin.User.PhoneNumberRequired")]
        [CustomStringLength("Admin.User.PhoneNumberLength", 12, 12)]
        public string PhoneNumber { get; set; }

        public List<LocalizationVM> Names { get; set; }

        [CustomDisplayName("Admin.User.Permissions")]
        public List<int> Permissions { get; set; }
    }
}