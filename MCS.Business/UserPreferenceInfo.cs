using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public  class UserPreferenceInfo
    {
        public int Id { get; set;}
        public int CultureId { get; set; }
        
        public bool IsDelegated { get; set; }
        public byte[] Signature { get; set; }
        public byte[] SignatureBehalf { get; set; }
        public byte[] SignatureCommand { get; set; }
        public byte[] Marking { get; set; }
        public byte[] MessageSignature { get; set; }
        public byte[] SealSignatureDoc { get; set; }
        public bool PasswordComfiration { get; set; }
        public Culture Culture { get; set; }
        public string Email { get; set; }
        public bool HasSignaturePasswordText { get; set; }
        public UserProfile UserProfile { get; set; }
        public NotificationSubscriptions NotificationSubscriptions { get; set; }
        public  IList<UserDelegation> UserDelegations { get; set; }
        public IList<UserTrayPreferenceInfo> UserTrayPreferencesInfo { get; set; }

        public int? FollowUpOrgId { get; set; }
        public int? FollowUpUserId { get; set; }
        public int ThemeId { get; set; }
        public bool SMSNotifications { get; set; }
        public IList<UserDelegation> MyDelegations { get; set; }
        public int DefaultDisplay { get; set; }
        public bool DefaultAssignmentPaper { get; set; }

    }
}
