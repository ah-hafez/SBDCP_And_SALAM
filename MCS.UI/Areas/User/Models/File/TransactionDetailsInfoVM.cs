using MCS.Common;
using System;

namespace MCS.UI.Areas.User.Models.File
{
    public class TransactionDetailsInfoVM
    {
        public int Id { get; set; }
        public string EncryptedId { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int Year { get; set; }
        public long Number { get; set; }
        public string DocumentNumber { get; set; }
        public string Remarks { get; set; }
        public string Subject { get; set; }
        public string SignedByUserName { get; set; }
        public int? SignedByUserId { get; set; }
        public string SignedByOrgUnitName { get; set; }
        public int? SignedByOrgUnitId { get; set; }
        public string ToEntityName { get; set; }
        public string ToUserName { get; set; }
        public string PriorityName { get; set; }
        public int? PriorityId { get; set; }
        public string ConfidentialityName { get; set; }
        public int? ConfidentialityId { get; set; }
        public string TransactionTypeName { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? TransactionTypeColorId { get; set; }
        public string LetterTypeName { get; set; }
        public string ExternalPartyName { get; set; }
        public int? ExternalPartyId { get; set; }
        public string ExternalPartyManagerName { get; set; }
        public int? ExternalPartyManagerId { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int TransactionCategoryId { get; set; }
        public string EncryptedTransactionCategoryId { get; set; }
        public string TransactionCategory { get; set; }
        public string EntityName { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string RejectionReason { get; set; }
        public int UserId { get; set; }
        public int? ToUserId { get; set; }
        public bool IsLate { get; set; }
        public bool isChecked { get; set; }
        public bool YasserRegistered { get; set; }
        public int AttachmentCount { get; set; }
        public int DestinationId { get; internal set; }
        public string SavedReason { get; set; }
        public int OrgUnitId { get; internal set; }
        public bool HasPermission { get; set; }
        public int DeliveryMethodId { get; set; }
        public int? TransactionPathId { get; set; }
        public bool IsIndividual { get; set; }
        public string DeliveryMethodName { get; set; }
        public string FollowupDateH { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string EncryptedIsDraft { get; set; }
        public bool HasLinks { get; set; }
        public int CopyStatus { get; set; }

        public string TransactionType { get; set; }

        public string SourceTypeName { get; set; }

        public HubDeliveryType DeliveryType { get; set; }
        public string DeliveryTypeName { get; set; }
        public string PrivecyName { get; set; }
        public int? PrivecyId { get; set; }
        public bool? isDeleted { get; set; } = false;
        public bool? IsPresentationDraft { get; set; }
        public bool? IsElcOutBound { get; set; }
        public bool SpecialCopy { get; set; }
        public bool IsBcc { get; set; }
        public bool IsOpr { get; set; }

        public int? OprEntityId { get; set; }
        public string OprEntityName { get; set; }
        public bool isImportant { get; set; }
        public bool HasTask { get; set; }
        public int? TransactionCopyId { get; set; }

        public bool Encrypted { get; set; }

    }
}