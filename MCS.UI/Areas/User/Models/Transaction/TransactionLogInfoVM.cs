using System.Collections.Generic;

namespace MCS.UI
{
    public class TransactionLogInfoVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<TransactionLogDetailInfoVM> TransactionLogDetails { get; set; }
    }
}
