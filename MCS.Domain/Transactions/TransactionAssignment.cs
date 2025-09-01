using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionAssignment : EntityBase, IAuditable
    {
        public int TrayId { get; set; }
        public virtual Tray Tray { get; set; }
        public int FromUserId { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public int? ToUserId { get; set; }
        public virtual UserProfile ToUser { get; set; }
        public int PhysicalUserId { get; set; }
        public virtual UserProfile PhysicalUser { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int? ActionId { get; set; }
        public virtual Action Action { get; set; }
        public int FromEntityId { get; set; }
        public virtual OrgUnit FromEntity { get; set; }
        public int ToEntityId { get; set; }
        public virtual OrgUnit ToEntity { get; set; }
        public int PhysicalEntityId { get; set; }
        public virtual OrgUnit PhysicalEntity { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public DateTime PhysicalDate { get; set; }
        public string PhysicalDateH { get; set; }

        public virtual IList<Task> Tasks { get; set; }
        public bool Viewed { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPopulariazation { get; set; }
        public int DeliveryMethodId { get; set; }
        public virtual Lookup DeliveryMethod { get; set; }
        public int? ReporterId { get; set; }
        public int? TransactionPathId { get; set; }
        public virtual TransactionPath TransactionPath { get; set; }
        public int? CurrentPathStep { get; set; }
        public DateTime DueDate { get; set; }

        [NotMapped]
        public int? UserDelegationId { get; set; }
        public DateTime? TransactionAssignmentProcessPeriod { get; set; }
        public string SpecialExplanation { get; set; }
        public string GeneralExplanation { get; set; }
    }
}
