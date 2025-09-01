using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionBasicInfoVM
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int TransactionCategoryId { get; set; }
        public long Number { get; set; }
        public string DocumentNumber { get; set; }
        public string Remarks { get; set; }
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Subject")]
        [CustomRequired("User.OutboundExternal.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundExternal.BasicInfo.SubjectLength", 255, 6)]
        public string Subject { get; set; }
        public string SignedByUserName { get; set; }

        public int? SignedByUserId { get; set; }
        public string SignedByOrgUnitName { get; set; }

        [CustomRequired("User.OutboundDraft.BasicInfo.SignedByOrgUnitRequired")]
        [CustomDisplayName("User.OutboundDraft.BasicInfo.SignedByOrgUnit")]
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

        [CustomDisplayName("User.OutboundExternal.BasicInfo.Type")]
        [CustomRequired("User.OutboundExternal.BasicInfo.TypeRequired")]
        public int TransactionTypeId { get; set; }

        public string LetterTypeName { get; set; }
        [CustomDisplayName("User.Transaction.BasicInfo.SubjectClassifications")]
        public int LetterTypeId { get; set; }
        public string ExternalPartyName { get; set; }

        [CustomRequired("User.OutboundDraft.BasicInfo.DestinationRequired")]
        [CustomDisplayName("User.OutboundDraft.BasicInfo.Destination")]
        public int ExternalPartyId { get; set; }

        public string ExternalPartyManagerName { get; set; }
        public int? ExternalPartyManagerId { get; set; }
        public int? OutboundDraftId { get; set; }
        public DateTime? RemindDate { get; set; }
        public string RemindDateH { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }

        public List<int> SubjectClassifications { get; set; }

        [CustomDisplayName("User.Transaction.BasicInfo.Suggestedtopic")]
        public int? SuggestedTopicId { get; set; }

        public bool IsSigned { get; set; }

        public int? OutboundDraftEditorType { get; set; }
        public string DeliveryMethod { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }

        [CustomDisplayName("User.Transaction.Name.PostCode")]
        [CustomStringLength("User.Transaction.Name.PostCodeLength", 12, 0)]
        public string PostCode { get; set; }

        [CustomDisplayName("User.Transaction.Name.POBox")]
        [CustomStringLength("User.Transaction.Name.POBoxLength", 12, 0)]
        public string POBox { get; set; }  //صندوق البريد//
        [CustomDisplayName("User.OutboundOpen.Source")]
        [CustomRequired("User.OutboundDraft.BasicInfo.SourceRequired")]
        public bool IsDraft { get; set; }
        public int? RecordNumber { get; set; }
        public string LetterNumber { get; set; }
        public List<TransactionLinkVM> Links { get; set; } = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionLinkVM>(), 1, 0, false);
        public List<TransactionAttachmentVM> Attachments { get; set; } = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAttachmentVM>(), 1, 0, false);
    }
}