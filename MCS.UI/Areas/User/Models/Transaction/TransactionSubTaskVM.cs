using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionSubTaskVM
    {
        public List<SubTaskAddVM> SubTasks { get; set; }
        public int TransactionId { get; set; }
        public int ParentId { get; set; }
    }
}