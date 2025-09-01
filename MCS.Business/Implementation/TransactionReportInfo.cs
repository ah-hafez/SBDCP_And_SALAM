using System.Collections.Generic;

namespace MCS.Business
{
    public class TransactionReportInfo
    {
        public int TransactionId { get; set; }
        public int? RejectReportId { get; set; }
        public List<int> ReportsIds { get; set; }
    }
}
