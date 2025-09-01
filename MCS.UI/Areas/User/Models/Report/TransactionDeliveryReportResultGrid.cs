using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionDeliveryReportResultGrid
    {
        public int PartyId { get; set; }

        public AjaxGrid<TransactionDeliveryReportVM> TransactionGridResultVMs { get; set; } = (AjaxGrid<TransactionDeliveryReportVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionDeliveryReportVM>(), 1, 0, false);
    }
}