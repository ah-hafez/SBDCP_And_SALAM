using System;
using System.Collections.Generic;
using MCS.Framework.Entities;
using MCS.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCS.Domain
{
    public class UserPreference : EntityBase
    {
        public int CultureId { get; set; }
        public virtual Culture Culture { get; set; }
        public bool IsDelegationEnabled { get; set; }
        public byte[] Signature { get; set; }
        public byte[] SignatureBehalf { get; set; }
        public byte[] SignatureCommand { get; set; }
        public byte[] MarkingDoc { get; set; }
        public byte[] MessageSignatureDoc { get; set; }
        public byte[] SealSignatureDoc { get; set; }
        public bool SignaturePassword { get; set; }
        public string SignaturePasswordText { get; set; }
        public bool HasSignaturePasswordText { get; set; }
        public string FreeText { get; set; }
        public int UserProfileId { get; set; }
        public string OTP { get; set; }
        public DateTime? OTPCreatedOn { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual IList<UserTrayPreference> UserTrayPreferences { get; set; }
        public NotificationSubscriptions NotificationSubscriptions { get; set; }
        public virtual IList<UserDelegation> UserDelegations { get; set; }
        public int? AssignmentPaperId { get; set; }
        public virtual AssignmentPaper AssignmentPaper { get; set; }
        public int? FollowUpOrgId { get; set; }
        public int? FollowUpUserId { get; set; }
        public virtual IList<UserPreferenceFollowup> UserPreferenceFollowups { get; set; }
        public int ThemeId { get; set; }
        public bool SMSNotifications { get; set; }
        public bool IsFollowUpUser { get; set; }
        public string PhoneNumber { get; set; }
        [NotMapped]
        public virtual IList<UserDelegation> MyDelegations { get; set; }
        public int DefaultDisplay { get; set; }

        public bool DefaultAssignmentPaper { get; set; }

    }
}
