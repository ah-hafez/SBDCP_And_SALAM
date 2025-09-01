using System;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionsDeliveryReportDTO
    {
        //[CustomDisplayName("User.Report.TransactionsDeliveryReport.TransactionType")]
        public int TransactionCategoryId { get; set; }

        public int? AssignedOrgUnitId { get; set; }

        public int? FromTransactionNumber { get; set; }

        public int? ToTransactionNumber { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int? MinuteFrom { get; set; }
        public int? MinuteTo { get; set; }

        public TimeSpan? TimeFrom { get; set; }

        public TimeSpan? TimeTo { get; set; }

        public int? LetterTypeId { get; set; }

        public int? PriorityLevelId { get; set; }

        public int? ConfidentialityLevelId { get; set; }

        public int? UserId { get; set; }

        public bool RePrint { get; set; }
        public int? DeliveryReportNumber { get; set; }
    }
}
