using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.File
{
    public class TransactionTrayInfoVM
    {
        public TransactionDetailsInfoVM TransactionDetailsInfoVM { get; set; }
      
        public IList<TransactionAssignmentInfoVM> TransactionAssignmentInfoVMs { get; set; }
        public ReceivedTaskVM ReceivedTaskVM { get; set; }
        public SentTaskVM SentTaskVM { get; set; }

        public FilterModel FilterModel { get; set; }
        public bool IsVIPUser { get; set; } = false;
        [CustomDisplayName("User.Transaction.AssigmentFrom")]
        public int FromEntityId { get; set; }
        

        [CustomDisplayName("User.Transaction.Assignment.ToOrgUnit")]
        public int? OrgUnitId { get; set; }
        [CustomDisplayName("User.Transaction.TransactionsAssignedTo")]
        public int? UserId { get; set; }
        public string UserName { get; set; }
    }
}