using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class SentTransactionReportResult
    {
        public int TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime AssignedDate { get; set; }
        public long? Number { get; set; }
        public string OrgUnitText { get; set; }
        public string TransactionCategoryText { get; set; }
        public string Subject { get; set; }
        public int TransactionCategoryId { get; set; }
        public int? ConfidentialityId { get; set; }
        public string ConfidentialityText { get; set; }
        public int? PriorityId { get; set; }
        public string PriorityText { get; set; }
        public string TransactionTypeText { get; set; }
        public int? FromEntityId { get; set; }
        public string FromEntityText { get; set; }
        public int? ToEntityId { get; set; }
        public string ToEntityText { get; set; }
        public string LetterTypeText { get; set; }
        public int TransactionStatusId { get; set; }
        public string TransactionStatusText { get; set; }
        public string TransactionElcOwner { get; set; }
        public string TransactionPhysicalOwner { get; set; }
        public bool Viewed { get; set; }

    }
}
