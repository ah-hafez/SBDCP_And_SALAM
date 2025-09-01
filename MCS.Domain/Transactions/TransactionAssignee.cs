using System;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionAssignee : EntityBase, IAuditable
    {
        public virtual Transaction Transaction { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual OrgUnit Entity { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
    }
}
