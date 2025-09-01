using System;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserDelegation : EntityBase
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateH { get; set; }
        public string ToDateH { get; set; }
        public int OrgUnitId { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public int UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public int? PriorityId { get; set; }
        public virtual Priority Priority { get; set; }
        public int? ConfidentialityId { get; set; }
        public virtual Permission Confidentiality { get; set; }
        public int? TransactionTypeId { get; set; }
        public virtual Lookup TransactionType { get; set; }
        public int UserPreferenceId { get; set; }
        public virtual UserProfile UserPreference { get; set; }
        public string RejectionReason { get; set; }
        public int StatusId { get; set; }
        public virtual Lookup Status { get; set; }
        public bool ReceiveCopy { get; set; }
        public bool ShowTransaction { get; set; }
        public string TransacionCategoryIds { get; set; }
        public string TransacionConfidentialityIds { get; set; }
    }
}
