using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionLogInfoDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<TransactionLogDetailInfoDTO> TransactionLogDetails { get; set; }
    }
}
