using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionsDeliveryReportVM
    {
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.TransactionType")]
        public int TransactionCategoryId { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.FromOrgUnit")]
        public int? FromOrgUnit { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.FromUser")]
        public int? FromUser { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.ToOrgUnit")]
        public int? ToOrgUnit { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.ToUser")]
        public int? ToUser { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.ToEntity")]
        public int? ToEntity { get; set; }
        [CustomNumberCompareAttribute("ToTransactionNumber", Operation.LessThan, "User.Report.TransactionsDeliveryReport.TransactionNumberCompare")]
        public int? FromTransactionNumber { get; set; }
        public int? ToTransactionNumber { get; set; }
        [CustomRequired("User.Report.TransactionsDeliveryReport.DateFromRequired")]
        [CustomDateTimeCompareAttribute("DateTo", Operation.LessThanOrEqual, "User.Report.TransactionsDeliveryReport.DateCompare")]
        public DateTime? DateFrom { get; set; }
        [CustomRequired("User.Report.TransactionsDeliveryReport.DateToRequired")]
        [CustomDateTimeCompareAttribute("DateFrom", Operation.GreaterThanOrEqual)]
        public DateTime? DateTo { get; set; }
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }
        //[CustomTimeSpanCompareAttribute("TimeTo", Operation.LessThan, "User.Report.TransactionsDeliveryReport.TimeCompare")]
        public TimeSpan? TimeFrom { get; set; }
        //[CustomTimeSpanCompareAttribute("TimeFrom", Operation.GreaterThan)]
        public TimeSpan? TimeTo { get; set; }
        //[CustomDisplayName("User.Transaction.BasicInfo.SubjectClassifications")]
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.LetterType")]
        public int? LetterTypeId { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.PriorityLevel")]
        public int? PriorityLevelId { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.ConfidentialityLevel")]
        public int? ConfidentialityLevelId { get; set; }
        [CustomDisplayName("User.Inbound.BasicInfo.InboundType")]
        public int? SourceId { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.RePrint")]
        public bool RePrint { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.User")]
        public int? UserId { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.ReportType")]
        public int ReportType { get; set; }
        [CustomDisplayName("User.Transaction.Outbound.Reporter")]
        public int? ReporterId { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.DeliveryReportNumber")]
        public int? DeliveryReportNumber { get; set; }
    }
}