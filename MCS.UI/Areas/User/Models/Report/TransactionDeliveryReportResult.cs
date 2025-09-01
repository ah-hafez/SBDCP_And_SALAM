using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionDeliveryReportResult
    {
        public Dictionary<int,string> Parties { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public int TransactionCategoryId { get; set; }
        public List<TransactionDeliveryReportResultGrid> transactionDeliveryReportResultGrids { get; set; }
    }
}