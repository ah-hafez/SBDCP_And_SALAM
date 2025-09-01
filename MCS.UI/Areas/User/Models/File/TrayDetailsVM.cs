using System.Collections.Generic;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.File
{
    public class TrayDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AllTransactionCount { get; set; }
        public int TodayTransactionCount { get; set; }
        private List<TransactionTrayInfoVM> transactionTrayInfoVMs;
        public List<TransactionTrayInfoVM> TransactionTrayInfoVMs { get; set; }
        public bool IsExcluded { get; set; }
        public bool IsVIPUser { get; set; }=false;

    }
}