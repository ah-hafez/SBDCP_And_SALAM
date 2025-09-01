using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.UserPreferences.UserDelegation;

namespace MCS.UI.Areas.User.Models.UserPreferences
{

    public class UserPreferenceVM
    {
        public UserPreferenceVM()
        {
            UserTrays = new List<UserTrayPreferencesVM>();
            CurrentDelegationUsers = new List<UserDelegationVM>();
            DelegationUsers = new List<AddUserDelegationVM>();
            NotificationSubscriptions = new List<NotificationSubscriptionVM>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        [CustomRequired("User.UserPreferences.LanguageRequired")]
        public int LanguageId { get; set; }

        [CustomEmailAddress("User.UserPreferences.InvalidEmail")]
        [CustomStringLength("User.UserPreferences.EmailLength", 50, 0)]
        public string Email { get; set; }
        public bool ActivateDelegation { get; set; }
        public bool PasswordForSignature { get; set; }
        public string SignaturePasswordText { get; set; }
        public SignType Signature { get; set; }
        public SignType SignatureCommand { get; set; }
        public SignType SignatureBehalf { get; set; }
        public SignType MessageSignature { get; set; }
        public SignType SealSignature { get; set; }
        public SignType Marking { get; set; }
        public byte[] SignatureDoc { get; set; }
        public byte[] SignatureBehalfDoc { get; set; }
        public byte[] MessageSignatureDoc { get; set; }
        public byte[] SealSignatureDoc { get; set; }
        public byte[] SignatureCommandDoc { get; set; }      
        public byte[] MarkingDoc { get; set; }
        public List<UserTrayPreferencesVM> UserTrays { get; set; }
        public List<UserDelegationVM> CurrentDelegationUsers { get; set; }
        public List<AddUserDelegationVM> DelegationUsers { get; set; }
        public List<NotificationSubscriptionVM> NotificationSubscriptions { get; set; }
        public int? AssignmentPaperId { get; set; }
        public AssignmentPaperVM AssignmentPaper { get; set; }
        public bool HasSignaturePasswordText { get; set; }

        [CustomRequired("Global.ResetPassword.NewPasswordRequierd")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [CustomRequired("Global.ResetPassword.ReNewPasswordRequierd")]
        [CustomCompare("NewPassword", "Global.ResetPassword.ReNewPasswordCompare")]
        public string ConfirmPassword { get; set; }

        [CustomDisplayName("User.UserPreferences.FollowUpOrgId")]
        public int? FollowUpOrgId { get; set; }
        [CustomDisplayName("User.UserPreferences.FollowUpUserId")]
        public int? FollowUpUserId { get; set; }
        [CustomRequired("User.UserPreferences.ThemeRequired")]
        public int Theme { get; set; }
     
        public bool SMSNotifications { get; set; }
        public bool IsFollowUpUser { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserDelegationVM> MyDelegations { get; set; }
        public int DefaultDisplay { get; set; }
        public bool DefaultAssignmentPaper { get; set; }
    }
}