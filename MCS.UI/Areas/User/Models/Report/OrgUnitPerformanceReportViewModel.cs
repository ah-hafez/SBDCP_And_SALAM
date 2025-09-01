using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class OrgUnitPerformanceReportViewModel
    {
        [CustomDisplayName("User.Report.UsersPerformance.OrgUnit")]
        [CustomRequired("User.Report.UsersPerformance.OrgUnitRequired")]
        public int OrgUnitId { get; set; }

        [CustomRequired("User.Report.UsersPerformance.DateFromRequired")]
        [CustomDateTimeCompareAttribute("ToDateTime", Operation.LessThanOrEqual, "User.Report.UsersPerformance.DateCompare")]
        public DateTime? FromDateTime { get; set; }

        [CustomRequired("User.Report.UsersPerformance.DateToRequired")]
        public DateTime? ToDateTime { get; set; }

        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        [CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThanOrEqual, "User.Report.UsersPerformance.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }

        [CustomTimeSpanCompareAttribute("TimeFrom", Operation.GreaterThanOrEqual)]
        public TimeSpan? TimeTo { get; set; }

        public bool IsStatusTransactionReport { get; set; }

    }
}
