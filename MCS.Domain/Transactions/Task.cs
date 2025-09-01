using System;
using System.Collections.Generic;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Task : EntityBase, IAuditable
    {
        public int ToUserId { get; set; }
        public virtual UserProfile ToUser { get; set; }
        public int ToOrgUnitId { get; set; }
        public virtual OrgUnit ToOrgUnit { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateH { get; set; }
        public bool IsExclusive { get; set; }
        public string TaskDescription { get; set; }
        public string StatusDescription { get; set; }
        public int? ParentId { get; set; }
        public virtual Task Parent { get; set; }
        public int StatusId { get; set; }
        public virtual Lookup Status { get; set; }
        public virtual List<TasksAttachments> TasksAttachments { get; set; }
        public int? LevelLimitation { get; set; }
        public int FromUserId { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public int FromOrgUnitId { get; set; }
        public virtual OrgUnit FromOrgUnit { get; set; }
        // public virtual IList<TaskWorkflow> TaskWorkflows { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual IList<TaskReminder> Reminders { get; set; }
        public virtual Action Action { get; set; }
        public int? ActionId { get; set; }
        public bool IsDeleted { get; set; }
        public int NumberOfNotifications { get; set; }
    }
}
