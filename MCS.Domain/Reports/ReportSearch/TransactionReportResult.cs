using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class TransactionReportResult
    {
        public int TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public DateTime Date { get; set; }
        public long? Number { get; set; }
        public string OrgUnitText { get; set; }
        public string TransactioDescription { get; set; }
        public string TransactionCategoryText { get; set; } 
        public int TransactionCategoryId { get; set; } 
        public int? ConfidentialityId { get; set; }
        public string ConfidentialityText { get; set; }
        public int? PriorityId { get; set; }
        public string PriorityText { get; set; }
        public int? SubjectClassificationId { get; set; }
        public string TransactionTypeText { get; set; }
        public string Remarks { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string DeliveryMethodText { get; set; }
        public string Subject { get; set; }
        public string FirstName { get; set; }
        public string CivilID { get; set; }
        public string MobileNumber { get; set; }
        public int? ExternalPartyId { get; set; }
        public string ExternalPartyText { get; set; }
        public string InboundDateH { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime OutBoundDate { get; set; }
        public int? FromEntityId { get; set; }
        public string FromEntityText { get; set; }
        public int? FromUserId { get; set; }
        public string FromUserText { get; set; }
        public int? ToEntityId { get; set; }
        public string ToEntityText { get; set; }
        public int? ToUserId { get; set; }
        public string ToUserText { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LetterTypeText { get; set; }
        public DateTime? RemindDate { get; set; }
        public int TransactionStatusId { get; set; }
        public string TransactionStatusText { get; set; }
        public string SavedReason { get; set; }
        public string DelayText { get; set; }
        public int DelayedDaysCount { get; set; }
        public DateTime AssignDate { get; set; }
        public int? SignedByUserId { get; set; }
        public string SignedByUserText { get; set; }
        public string LetterNumber { get; set; }

    }
}
