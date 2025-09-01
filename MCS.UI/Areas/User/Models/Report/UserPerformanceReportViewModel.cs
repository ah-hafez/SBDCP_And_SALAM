using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.UI.Areas.User.Models
{
   public class UserPerformanceReportViewModel
    {
        [CustomDisplayName("User.Report.UsersPerformance.OrgUnit")]
        [CustomRequired("User.Report.UsersPerformance.OrgUnitRequired")]
        public int OrgUnitId { get; set; }

         [CustomDisplayName("User.Report.UsersPerformance.User")]
        public int? UserId { get; set; }

        [CustomRequired("User.Report.UsersPerformance.DateFromRequired")]
        [CustomDateTimeCompareAttribute("ToDateTime", Operation.LessThanOrEqual, "User.Report.UsersPerformance.DateCompare")]
         public DateTime? FromDateTime { get; set; }

        [CustomRequired("User.Report.UsersPerformance.DateToRequired")]
        //[CustomDateTimeCompareAttribute("DateFrom", Operation.GreaterThanOrEqual)]
        public DateTime? ToDateTime { get; set; }

        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        [CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThanOrEqual, "User.Report.UsersPerformance.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }

        [CustomTimeSpanCompareAttribute("TimeFrom", Operation.GreaterThanOrEqual)]
        public TimeSpan? TimeTo { get; set; }

        public bool IsGeneralReport { get; set; }
   
    }
}
