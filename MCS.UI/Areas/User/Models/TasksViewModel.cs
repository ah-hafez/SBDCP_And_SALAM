using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models
{
    public class TasksViewModel
    {
        public List<ReceivedTaskVM> ReceivedTaskVMs { get; set; }

        public int ReceivedTasksCount { get; set; }

        public List<SentTaskVM> SentTaskVMs { get; set; }

        public int SentTasksCount { get; set; }
    }
}