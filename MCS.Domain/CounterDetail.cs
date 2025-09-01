
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class CounterDetail : EntityBase
    {
        public int InitialValue { get; set; }
        public int Count { get; set; }
        public TransactionCategories TransactionCategories { get; set; }
        public int? TransactionTypeId { get; set; }

        public virtual Counter Counter { get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }
}
