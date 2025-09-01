using MCS.DTO;
using System.Collections.Generic;

namespace MCS.Business
{
    public class TransactionTrayInfo
    {
        public TransactionDetailsInfo transactionDetailsInfo { get; set; }
        public IList<TransactionAssignmentInfo> TransactionAssignmentInfos { get; set; }
    }
}
