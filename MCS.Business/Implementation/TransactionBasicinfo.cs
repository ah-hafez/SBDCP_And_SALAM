using MCS.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MCS.Business
{
    public class TransactionBasicInfo
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
        public int SignedByOrgUnitId { get; set; }
        public string ToEntityName { get; set; }
        public string ToUserName { get; set; }
        public string PriorityName { get; set; }
        public int PriorityId { get; set; }
        public string ConfidentialityName { get; set; }
        public int ConfidentialityId { get; set; }
        public string TransactionTypeName { get; set; }
        public int TransactionTypeId { get; set; }
        public string LetterTypeName { get; set; }
        public int LetterTypeId { get; set; }
        public string ExternalPartyName { get; set; }
        public int ExternalPartyId { get; set; }
        public string ExternalPartyManagerName { get; set; }
        public int? ExternalPartyManagerId { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int TransactionCategoryId { get; set; }
        public int? OutboundDraftId { get; set; }
        public List<int> SubjectClassifications { get; set; }
        public int? SuggestedTopicId { get; set; }
        public bool IsSigned { get; set; }
        public int? OutboundDraftEditorType { get; set; }
        public string DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public string PostCode { get; set; }
        public string POBox { get; set; }
        public string TransactionType { get; set; }
        public string StatusName { get; set; }
        public int YearH { get; set; }
        public string LetterNumber { get; set; }
        public virtual IList<TransactionLink> Links { get; set; }
        public virtual IList<Attachment> Attachments { get; set; }
    }
}
