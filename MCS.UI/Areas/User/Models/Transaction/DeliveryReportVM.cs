using System.Collections.Generic;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class DeliveryReportVM
    {
        public string OrgUnitName { get; set; }
        public string ReportNumber { get; set; }
        public string DateH { get; set; }
        public string RootName { get; set; }
        public string UserName { get; set; }
        public string ReporterName { get; set; }
        public TransactionBarcodesVM  transactionBarcodesVM { get; set; }
        public List<DeliveryReportTransactionVM> DeliveryReportTransactions { get; set; }
        public List<DeliveryReportTransactionVM> Transactions { get; set; } = (AjaxGrid<DeliveryReportTransactionVM>)new AjaxGridFactory().CreateAjaxGrid(new List<DeliveryReportTransactionVM>(), 1, 0, false);
        public string ConfidentialityName { get; internal set; }
        public string TransactionTypeName { get; internal set; }
    }
}