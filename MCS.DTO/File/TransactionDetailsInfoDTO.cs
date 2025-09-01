using System;

namespace MCS.DTO
{
    public class TransactionDetailsInfoDTO
    {
        public int Id { get; set; }
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
        public string TransactionCategory { get; set; }
        public string EntityName { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string RejectionReason { get; set; }
        public int UserId { get; set; }
        public int? ToUserId { get; set; }
        public bool IsLate { get; set; }
        public bool YasserRegistered { get; set; }
        public byte[] Exponent { get; set; }
        public byte[] Modulus { get; set; }
        public int AttachmentCount { get; set; }
        public string SavedReason { get; set; }
        public bool HasPermission { get; set; }
        public int DeliveryMethodId { get; set; }
        public int? TransactionPathId { get; set; }
        public bool IsIndividual { get; set; }
        public string DeliveryMethodName { get; set; }
        public string FollowupDateH { get; set; }
        public DateTime? FollowupDate { get; set; }
        public bool HasLinks { get; set; }
        public int CopyStatus { get; set; }
        public string PrivecyName { get; set; }
        public int? PrivecyId { get; set; }
        public bool? isDeleted { get; set; }
        public bool? IsPresentationDraft { get; set; }
        public bool? IsElcOutBound { get; set; }
        public bool SpecialCopy { get; set; }
        public bool IsOpr { get; set; }
        public bool IsBcc { get; set; }

        public int? OprEntityId { get; set; }
        public string OprEntityName { get; set; }
        public bool IsImportant { get; set; }
        public int? TransactionCopyId { get; set; }
        public bool HasTask { get; set; }
        public bool Encrypted { get; set; }


    }
}
