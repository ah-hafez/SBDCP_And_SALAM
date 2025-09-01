using System;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class TransactionAssignmentHistory : EntityBase
    {
        public int TrayId { get; set; }
        public virtual Tray Tray { get; set; }
        public int FromUserId { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public int? ToUserId { get; set; }
        public virtual UserProfile ToUser { get; set; }
        public int? TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int? ActionId { get; set; }
        public virtual Action Action { get; set; }
        public int FromEntityId { get; set; }
        public virtual OrgUnit FromEntity { get; set; }
        public int ToEntityId { get; set; }
        public virtual OrgUnit ToEntity { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int? ExplanationId { get; set; }
        public virtual Explanation Explanation { get; set; }
        public int? UserDelegationId { get; set; }
        public virtual UserDelegation UserDelegation { get; set; }
        public virtual bool Viewed { get; set; }
        public bool IsHidden { get; set; }
        public string SpecialExplanation { get; set; }
        public string GeneralExplanation { get; set; }
        [NotMapped]
        public string ReceivedDate { get; set; }

    }
}
