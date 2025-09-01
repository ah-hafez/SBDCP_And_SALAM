using System;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCS.Domain
{
    public class TransactionCopy : EntityBase, IAuditable
    {
        public int? UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public int? EntityId { get; set; }
        public virtual OrgUnit Entity { get; set; }
        public int? FromUserId { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public int? FromEntityId { get; set; }
        public virtual OrgUnit FromEntity { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int Status { get; set; }
        public virtual Action Action { get; set; }
        public int ActionId { get; set; }
        public int? IsSent { get; set; }

        [NotMapped]
        public bool SendEmail { get; set; }
        public DateTime? SentDate { get; set; }
        public bool Viewed { get; set; }
        public string SpecialExplanation { get; set; }
        public string GeneralExplanation { get; set; }
        public bool IsOpr { get; set; }
        public bool IsBcc { get; set; }
        public bool SpecialCopy { get; set; }
        public int? OprEntityId { get; set; }
        public virtual OrgUnit OprEntity { get; set; }
        public DateTime? ViewedOnDate { get; set; }
        public string ViewedOnDateH { get; set; }
        public virtual UserProfile ViewedBy { get; set; }
        public int? ViewedById { get; set; }

    }
}
