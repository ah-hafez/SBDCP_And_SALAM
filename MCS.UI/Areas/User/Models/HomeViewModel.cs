using System.Collections.Generic;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models
{
    public class HomeViewModel
    {
        public List<TrayDetailsVM> TrayDetails { get; set; }
        public List<TaskStatusVM> TasksStatus { get; set; }
        public List<TransactionTrayInfoVM> TransactionTrayInfos { get; set; }
    }
}