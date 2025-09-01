using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionReportInfoDTO
    {
        public int TransactionId { get; set; }
        public int? RejectReportId { get; set; }
        public List<int> ReportsIds { get; set; }
    }
}
