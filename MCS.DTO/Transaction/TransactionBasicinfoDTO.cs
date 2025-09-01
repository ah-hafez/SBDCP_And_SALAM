using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionBasicInfoDTO 
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int TransactionCategoryId { get; set; }
        public long Number { get; set; }
        public string DocumentNumber { get; set; }
        public string Remarks { get; set; }

        [CustomRequired("User.OutboundDraft.BasicInfo.SubjectRequired")]
        public string Subject { get; set; }
        public string SignedByUserName { get; set; }

        public int? SignedByUserId { get; set; }
        public string SignedByOrgUnitName { get; set; }

        public int SignedByOrgUnitId { get; set; }

        public string ToEntityName { get; set; }
        public string ToUserName { get; set; }
        public string PriorityName { get; set; }

        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityId { get; set; }
        public string ConfidentialityName { get; set; }

        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityId { get; set; }
        public string TransactionTypeName { get; set; }

        [CustomRequired("User.OutboundDraft.BasicInfo.SourceRequired")]
        public int TransactionTypeId { get; set; }

        public string LetterTypeName { get; set; }

        [CustomRequired("User.OutboundDraft.BasicInfo.TypeRequired")]
        public int LetterTypeId { get; set; }
        public string ExternalPartyName { get; set; }

        [CustomRequired("User.OutboundDraft.BasicInfo.DestinationRequired")]
        public int ExternalPartyId { get; set; }

        public string ExternalPartyManagerName { get; set; }
        public int? ExternalPartyManagerId { get; set; }
        public int? OutboundDraftId { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }

        //[CustomDisplayName("User.Transaction.BasicInfo.SubjectClassifications")]
        public List<int> SubjectClassifications { get; set; }

        //[CustomDisplayName("User.Transaction.BasicInfo.SuggestedTopic")]
        public int? SuggestedTopicId { get; set; }

        public bool IsSigned { get; set; }

        public int? OutboundDraftEditorType { get; set; }
        public string DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public string PostCode { get; set; }
        public string POBox { get; set; }  //صندوق البريد//

        public string CurrentOrgUnit { get; set; }
        public string StatusName { get; set; }
        public int YearH { get; set; }
        public int? RecordNumber { get; set; }
        public string LetterNumber { get; set; }
        public List<TransactionAttachmentDTO> Attachments { get; set; }
        public List<TransactionLinkDTO> Links { get; set; }
    }
}
