using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionTaskVM
    {
        public List<TaskAddVM> TaskVMs { get; set; }
        public int TransactionId { get; set; }
    }
}