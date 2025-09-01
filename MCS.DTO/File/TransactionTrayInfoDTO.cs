using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionTrayInfoDTO
    {
       public TransactionDetailsInfoDTO TransactionDetailsInfoDTOs { get; set; }
       public IList<TransactionAssignmentInfoDTO> TransactionAssignmentInfoDTOs { get; set; }
       public SentTaskDTO sentTasDTO { get; set; }
    }
}
