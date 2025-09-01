
using System;

namespace MCS.Domain
{
    public class TransactionCountReportInfo
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public DateTime Date { get; set; }
        public int UserCategoryId { get; set; }
    }
}
