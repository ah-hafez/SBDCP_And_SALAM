using MCS.UI.Areas.User.Models.File;
using System;
using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Search
{
    public class BaseSearchResultVM
    {
        public int Id { get; set; }
        public string EncryptedId { get; set; }
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
        public bool WithArchiving { get; set; }
        public int ColorCode { get; set; }
        public string TransactionCategoryName { get; set; }
        public int TransactionCategoryId { get; set; }
        public string EncryptedTransactionCategoryId { get; set; }
        public bool HasPermission { get; set; }
        public int? ToUserId { get; set; }
        public int? ToEntityId { get; set; }
        public int StatusId { get; set; }
        public int IsDeleted { get; set; }
        public int? TotalCount { get; set; }
        public int DeliveryMethodId { get; set; }
        public bool HasLinks { get; set; }
        public int? ConfidentialityId { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int? TransactionTypeId { get; set; }
        public TransactionAssignmentInfoVM TransactionAssignmentInfoVM { get; set; }
        public bool Encrypted { get; set; }
        public string DocumentNumber { get; set; }
    }
}