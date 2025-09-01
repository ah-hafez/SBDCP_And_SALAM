using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class OutboundInternalDashboardViewModel
    {
        [CustomDisplayName("User.Report.InboundTransactions.OrgUnit")]
        [CustomRequired("User.Report.InboundTransactions.OrgUnitRequired")]
        public int OrgUnitId { get; set; }

        [CustomRequired("User.Report.InboundTransactions.DateFromRequired")]
        [CustomDateTimeCompareAttribute("ToDateTime", Operation.LessThanOrEqual, "User.Report.InboundTransactions.DateCompare")]
        public DateTime? FromDateTime { get; set; }

        [CustomRequired("User.Report.InboundTransactions.DateToRequired")]
        public DateTime? ToDateTime { get; set; }

        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        [CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThanOrEqual, "User.Report.InboundTransactions.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }

        [CustomDisplayName("User.Report.OutboundInternal.Source")]
        public int? SourceId { get; set; }

        [CustomDisplayName("User.Report.InboundTransactions.PriorityLevel")]
        public int? PriorityId { get; set; }

        [CustomDisplayName("User.Report.InboundTransactions.ConfidentialityLevel")]
        public int? ConfidentialityId { get; set; }

        public int TransactionTypeId { get; set; }
    }
}
