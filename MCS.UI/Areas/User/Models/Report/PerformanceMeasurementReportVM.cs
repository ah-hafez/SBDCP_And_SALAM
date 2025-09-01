using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class PerformanceMeasurementReportVM
    {
        [CustomRequired("User.ReportTypeRequired")]
        public int ReportTypeId { get; set; }

        [CustomRequired("User.RepresentationTypeRequired")]
        public int RepresentationTypeId { get; set; }

        [CustomDisplayName("User.Copy.OrgUnitName")]
        public int OrgUnitId { get; set; }
                
        [CustomDateTimeCompareAttribute("To", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime From { get; set; }

        [CustomDateTimeCompareAttribute("From", Operation.GreaterThanOrEqual)]
        public DateTime To { get; set; }
        public int Level { get; set; }
        public List<int> ColumnsToGrid { get; set; }
        public int TotalCount { get; set; }
        public bool? IsPrint { get; set; }
        public int DepartmentId { get; set; }
        public string ummalqura { get; set; }
        public string gregorian { get; set; }

        #region Common
        public CommonVM CommonVM { get; set; } = new CommonVM();
        #endregion

        #region EmployeeVM
        public EmployeeVM EmployeeVM { get; set; } = new EmployeeVM(); 
        #endregion
    }
}