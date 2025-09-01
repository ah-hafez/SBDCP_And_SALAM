
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class LookupBase : EntityBase, IAuditable
    {
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        public TransactionCategories TransactionCategories { get; set; }
        public bool IsInternal { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }

    }
}
