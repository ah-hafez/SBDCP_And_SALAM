using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionGridResultVM
    {
        public int TransactionId { get; set; }
        public string EncryptedId { get; set; }
        public int TransactionTypeId { get; set; }
        public int TransactionCategoryId { get; set; }

        public string TransactionCategoryText { get; set; }
        public string OrgUnitText { get; set; }
        public DateTime Date { get; set; }
        public string DateText { get; set; }
        public long Number { get; set; }
        public string TransactioDescription { get; set; }
        public string ConfidentialityText { get; set; }
        public string PriorityText { get; set; }
        public string LetterTypeText { get; set; }
        public string Remarks { get; set; }
        public string DeliveryMethodText { get; set; }
        public string Subject { get; set; }
        public string FirstName { get; set; }
        public string CivilID { get; set; }
        public string MobileNumber { get; set; }
        public string ExternalPartyText { get; set; }
        public string InboundDateH { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime OutBoundDate { get; set; }
        public string FromEntityText { get; set; }
        public string FromUserText { get; set; }
        public string ToEntityText { get; set; }
        public string ToUserText { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RemindDate { get; set; }
        public string TransactionTypeText { get; set; }
        public string EncryptedIsDraft { get; set; }

        public int ToUserId { get; set; }

        public string TransactionStatus { get; set; }
        public string SavedReason { get; set; }
        public string DelayText { get; set; }
        public string NumberWithDate { get; set; }
        public string DelayedDaysCount { get; set; }
        public DateTime AssignDate { get; set; }
        public int? SignedByUserId { get; internal set; }
        public string SignedByUserText { get; internal set; }
        public int? ConfidentialityId { get; internal set; }
        public string LetterNumber { get; set; }
        public bool HasPermission { get; set; }
    }
}