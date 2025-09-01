using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class OutboundDashboardViewModel
    {
        [CustomDisplayName("User.Report.OutboundTransactions.OrgUnit")]
        [CustomRequired("User.Report.OutboundTransactions.OrgUnitRequired")]
        public int OrgUnitId { get; set; }

        [CustomRequired("User.Report.OutboundTransactions.DateFromRequired")]
        [CustomDateTimeCompareAttribute("ToDateTime", Operation.LessThanOrEqual, "User.Report.OutboundTransactions.DateCompare")]
        public DateTime? FromDateTime { get; set; }

        [CustomRequired("User.Report.OutboundTransactions.DateToRequired")]
        public DateTime? ToDateTime { get; set; }

        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        [CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThanOrEqual, "User.Report.OutboundTransactions.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }

        public TimeSpan? TimeTo { get; set; }

        [CustomDisplayName("User.Report.OutboundTransactions.Source")]
        public int? SourceId { get; set; }

        [CustomDisplayName("User.Report.OutboundTransactions.Destination")]
        public int? DestinationId { get; set; }

        [CustomDisplayName("User.Report.OutboundTransactions.ConfidentialityLevel")]
        public int? ConfidentialityId { get; set; }

        [CustomDisplayName("User.Report.OutboundTransactions.PriorityLevel")]
        public int? PriorityId { get; set; }
    }
}
