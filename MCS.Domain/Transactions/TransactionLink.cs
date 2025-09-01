using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionLink : EntityBase, IAuditable
    {
        public int TypeId { get; set; }
        public virtual Link Type { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual Transaction ToTransaction { get; set; }
        public int ToTransactionId { get; set; }
    }
}
