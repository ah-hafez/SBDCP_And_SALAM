using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.Admin.Models;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionReportResult
    {
        public RepresentationReportType RepresentationReportType { get; set; }
        public TransactionBasicResultVM TransactionBasicResultVM { get; set; } = new TransactionBasicResultVM();
        public AjaxGrid<TransactionGridResultVM> TransactionGridResultVMs { get; set; } = (AjaxGrid<TransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionGridResultVM>(), 1, 0, false);
        public List<TransactionGridResultVM> TransactionPrintResultVMs { get; set; } = new List<TransactionGridResultVM>();

        public AjaxGrid<PerformanceMeasurementGridResultVM> PerformanceMeasurementGridResultVMs { get; set; } = (AjaxGrid<PerformanceMeasurementGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(new List<PerformanceMeasurementGridResultVM>(), 1, 0, false);
        public AjaxGrid<UserGroupVM> UserGroupDTOtGridResultVMs { get; set; } = (AjaxGrid<UserGroupVM>)new AjaxGridFactory().CreateAjaxGrid(new List<UserGroupVM>(), 1, 0, false);
        public AjaxGrid<UserProfileVM> UsersDTOtGridResultVMs { get; set; } = (AjaxGrid<UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(new List<UserProfileVM>(), 1, 0, false);

    }
}