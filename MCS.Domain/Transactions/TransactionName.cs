using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionName : EntityBase, IAuditable
    {
        public int TransactionId { get; set; }
        public int NameId { get; set; }
        public virtual Name Name { get; set; }
    }
}
