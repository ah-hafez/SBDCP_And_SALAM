using System;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionFollowUp : EntityBase, IAuditable
    {
        public int TransactionId { get; set; }
        public int CreatingUserId { get; set; }
        public virtual UserProfile CreatingUser { get; set; }
        public int CreatingEntityId { get; set; }
        public virtual OrgUnit CreatingEntity { get; set; }
        public int FollowUpEntityId { get; set; }
        public virtual OrgUnit FollowUpEntity { get; set; }
        public int? FollowUpUserId { get; set; }
        public virtual UserProfile FollowUpUser { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime FollowUpExpireDate { get; set; }
        public string FollowUpExpireDateHj { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public int ProccessPeriod { get; set; }
        public DateTime? ProccessPeriodDate { get; set; }

        public string FollowUpProccessNote { get; set; }

        public DateTime? FollowUpCompletionDate { get; set; }
        public string FollowUpCompletionDateHj { get; set; }
        public DateTime? FollowUpReceiveDate { get; set; }

        public string FollowUpReason { get; set; }

        public int FollowUpTypeId { get; set; }

        public int FollowUpStatusId { get; set; }
         
        public int FollowUpMethodId { get; set; }
         
        public int FollowUpPriortyId { get; set; }  
        public int FollowUpProccessId { get; set; }  
        public int FollowUpSourceId { get; set; }

        public int? FollowUpProgressId { get; set; }
        public DateTime? DateTo { get; set; }
        public string DateToH { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCopy { get; set; }
        public bool IsReminder { get; set; }
        public bool IsEscalated { get; set; }
        public bool IsImportant { get; set; }
        public bool HasChild { get; set; }
        public int? ParentId { get; set; }
         
    }
}