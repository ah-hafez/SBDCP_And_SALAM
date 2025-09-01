using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class CounterDetailDTO
    {
        public int Id { get; set; }
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
        public int InitialValue { get; set; }
        public int Count { get; set; }
        public int LastTransactionNumber { get; set; }
    }
}
