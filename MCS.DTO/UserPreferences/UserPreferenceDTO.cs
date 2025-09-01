using System.Collections.Generic;
using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class UserPreferenceDTO
    {
        public UserPreferenceDTO()
        {
            UserTrays = new List<UserTrayPreferencesDTO>();
            CurrentDelegationUsers = new List<UserDelegationDTO>();
            DelegationUsers = new List<AddUserDelegationDTO>();
            NotificationSubscriptions = new List<NotificationSubscriptionDTO>();
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
        public SignType SignatureCommand { get; set; }
        public SignType Signature { get; set; }
        public SignType SignatureBehalf { get; set; }
        public SignType Marking { get; set; }
        public SignType MessageSignature { get; set; }
        public SignType SealSignature { get; set; }
        public byte[] SealSignatureDoc { get; set; }
        public byte[] MessageSignatureDoc { get; set; }
        public byte[] SignatureDoc { get; set; }
        public byte[] SignatureBehalfDoc { get; set; }
        public byte[] SignatureCommandDoc { get; set; }
        public byte[] MarkingDoc { get; set; }
        public List<UserTrayPreferencesDTO> UserTrays { get; set; }
        public List<UserDelegationDTO> CurrentDelegationUsers { get; set; }
        public List<AddUserDelegationDTO> DelegationUsers { get; set; }
        public List<NotificationSubscriptionDTO> NotificationSubscriptions { get; set; }
        public int? AssignmentPaperId { get; set; }
        public AssignmentPaperDTO AssignmentPaper { get; set; }
        public bool HasSignaturePasswordText { get; set; }
        public int? FollowUpOrgId { get; set; }
        public int? FollowUpUserId { get; set; }
        public int Theme { get; set; }
        public bool SMSNotifications { get; set; }
        public bool IsFollowUpUser { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserDelegationDTO> MyDelegationUsers { get; set; }
        public int DefaultDisplay { get; set; }
        public bool DefaultAssignmentPaper { get; set; }

    }
}

