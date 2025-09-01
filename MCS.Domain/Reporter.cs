using System.Collections.Generic;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Reporter : EntityBase, IAuditable
    {
        public int ToEntityId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public string Text { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        public virtual IList<TransactionDeliveryReport> TransactionDeliveryReports { get; set; }

    }
}
