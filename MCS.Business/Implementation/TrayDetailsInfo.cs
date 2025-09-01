using System.Collections.Generic;

namespace MCS.Business
{
    public class TrayDetailsInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AllTransactionCount { get; set; }
        public int TodayTransactionCount { get; set; }
        public IList<TransactionTrayInfo> TransactionTraysInfo { get; set; }
    }
}
