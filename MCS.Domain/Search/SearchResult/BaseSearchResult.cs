using System;

namespace MCS.Domain
{
    public class BaseSearchResult
    {
        public int Id { get; set; }
        public string TransactionType { get; set; }
        public long Number { get; set; }
        public string Subject { get; set; }
        public string DateH { get; set; }
        public DateTime Date { get; set; }
        public string ConfidentialityName { get; set; }
        public string PriorityName { get; set; }
        public string PartyName { get; set; }
        public string OrgUnitName { get; set; }
        public string StatusName { get; set; }
        public int WithArchiving { get; set; }
        public int ColorCode { get; set; }
        public string TransactionCategoryName { get; set; }
        public int TransactionCategoryId { get; set; }
        public bool HasPermission { get; set; }
        public int? Weight { get; set; }
        public int? ToUserId { get; set; }
        public int? ToEntityId { get; set; }        
        public int StatusId { get; set; }
        public int IsDeleted { get; set; }
        public int? TotalCount { get; set; }
        public int HasLinks { get; set; }
        public int? ConfidentialityId { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int? TransactionTypeId { get; set; }
        public TransactionAssignment TransactionAssignment { get; set; }
        public bool IsView { get; set; } = false;
        public bool Encrypted { get; set; }
        public string DocumentNumber { get; set; }
    }
}
