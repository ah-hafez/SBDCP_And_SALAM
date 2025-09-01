using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AddOutboundDraftBasicInfoDTO : BasicInfoBaseDTO
    {
        //[CustomDisplayName("User.OutboundDraft.BasicInfo.DraftNumber")]
        public int? DraftNumber { get; set; }    //رقم مشروع الصادر//

        //[CustomDisplayName("User.OutboundDraft.BasicInfo.Source")]
        [CustomRequired("User.OutboundDraft.BasicInfo.SourceRequired")]
        public int TransactionTypeId { get; set; }   //نوع الصادر//

        //[CustomDisplayName("User.OutboundDraft.BasicInfo.Destination")]
        [CustomRequired("User.OutboundDraft.BasicInfo.DestinationRequired")]
        public int DestinationId { get; set; }    //جهة الصادر//
        public int? ExternalPartyId { get; set; }    //جهة الصادر الخارجي//


        //[CustomDisplayName("User.OutboundDraft.BasicInfo.DirectedTo")]
        public int? DirectedToId { get; set; }   //موجهة الى//

        //[CustomDisplayName("User.OutboundDraft.BasicInfo.SignedByOrgUnit")]
        [CustomRequired("User.OutboundDraft.BasicInfo.SignedByOrgUnitRequired")]
        public int SignedByOrgUnitId { get; set; }  

        //[CustomDisplayName("User.OutboundDraft.BasicInfo.SignedBy")]
        public int? SignedById { get; set; } //موقعة من//

        [CustomRequired("User.OutboundExternal.BasicInfo.PreparationEntityRequired")]
        public int PreparationEntityId { get; set; }    //الادارة المعدة للصادر//

        //[CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //مستوى السريه//

        //[CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; } //درجة الأسبقية//

        //[CustomDisplayName("User.OutboundDraft.BasicInfo.Subject")]
        [CustomRequired("User.OutboundDraft.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundDraft.BasicInfo.SubjectLength", 500)]
        public string Subject { get; set; } //الموضوع//

        [CustomDisplayName("User.OutboundInternal.BasicInfo.Summary")]
        //[CustomRequired("User.OutboundInternal.BasicInfo.SummaryRequired")]
        [CustomStringLength("User.OutboundInternal.BasicInfo.SummaryLength", 2000, 6)]
        public string Summary { get; set; } //الملخص// 

        //[CustomDisplayName("User.OutboundDraft.BasicInfo.Type")]
        [CustomRequired("User.OutboundDraft.BasicInfo.TypeRequired")]
        public int LetterTypeId { get; set; } //نوع خطاب الصادر//
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
        public bool IsDraft { get; set; }

        public int? ReporterId { get; set; }

        public int? TransactionPathId { get; set; }
        public int? SubjectClassificationsId { get; set; }   //درجة السريه//

        public bool isOutboundInternalDraft { get; set; }
        public int? privacyLevelId { get; set; }
        public string LetterNumber { get; set; }
        public bool IsPresentationDraft { get; set; }
        public long? PresentationDraftNumber { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public bool NeedAcknowled { get; set; }
        public bool IsDecisionDraft { get; set; }
        public bool IsMultiExternal { get; set; }
        public bool Encrypted { get; set; }
    }
}