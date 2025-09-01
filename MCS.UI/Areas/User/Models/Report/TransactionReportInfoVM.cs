using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionReportInfoVM
    {
        public int TransactionId { get; set; }
        public List<int> ReportsIds { get; set; }
    }
}