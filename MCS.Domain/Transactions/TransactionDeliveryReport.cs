using System;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionDeliveryReport : EntityBase
    {
        public int UserId { get; set; }
        public int OrgunitId { get; set; }
        public string Number { get; set; }
        public int? TransactionAssignmentHistoryId { get; set; }
        public virtual TransactionAssignmentHistory TransactionAssignmentHistory { get; set; }
        public int? TransactionHistoryId { get; set; }
        public virtual TransactionHistory TransactionHistory { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int? DocumentId { get; set; }
        public virtual DocumentInfo Document { get; set; }
        public int? ReporterId { get; set; }
        public virtual Reporter Reporter { get; set; }
        public int? TransactionExternalCopyId { get; set; }
        public virtual TransactionExternalCopy TransactionExternalCopy { get; set; }
        public int? TransactionCopyId { get; set; }
        public virtual TransactionCopy TransactionCopy { get; set; }
    }
}
