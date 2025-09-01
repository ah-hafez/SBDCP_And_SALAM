using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionEntityDetails : EntityBase
    {
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int EntityId { get; set; }
        public virtual OrgUnit Entity { get; set; }
    }
}
