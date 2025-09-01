using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Report
{
    public class DeliveryReportInfoVM
    {
        public string OrgUnitName { get; set; }
        public string ReportNumber { get; set; }
        public string DateH { get; set; }
        public string RootOrgUnitName { get; set; }
        public string UserName { get; set; }
        public IList<DeliveryReportTransactionInfoVM> DeliveryReportTransactions { get; set; }
    }
}