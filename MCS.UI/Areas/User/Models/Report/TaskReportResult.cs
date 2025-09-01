using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TaskReportResult
    {
        public RepresentationReportType RepresentationReportType { get; set; }
        public TransactionBasicResultVM TransactionBasicResultVM { get; set; } = new TransactionBasicResultVM();
        public AjaxGrid<TaskGridResultVM> TransactionGridResultVMs { get; set; } = (AjaxGrid<TaskGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskGridResultVM>(), 1, 0, false);
        public List<TaskGridResultVM> TransactionPrintResultVMs { get; set; } = new List<TaskGridResultVM>();

        public AjaxGrid<PerformanceMeasurementGridResultVM> PerformanceMeasurementGridResultVMs { get; set; } = (AjaxGrid<PerformanceMeasurementGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(new List<PerformanceMeasurementGridResultVM>(), 1, 0, false);
        public AjaxGrid<UserGroupVM> UserGroupDTOtGridResultVMs { get; set; } = (AjaxGrid<UserGroupVM>)new AjaxGridFactory().CreateAjaxGrid(new List<UserGroupVM>(), 1, 0, false);
    }
}