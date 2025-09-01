using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class EditOutboundExternalBasicInfoDTO : BasicInfoBaseDTO
    {
        //[CustomDisplayName("User.OutboundExternal.BasicInfo.OutboundNumber")]
        public long OutboundNumber { get; set; } //رقم الصادر//

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.Source")]
        public int TransactionTypeId { get; set; }   //نوع الصادر//

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.Destination")]
        [CustomRequired("User.OutboundExternal.BasicInfo.DestinationRequired")]
        public int DestinationId { get; set; }    //جهة الصادر//
        public int ExternalPartyId { get; set; }    //جهة الصادر//

        public string PriorityLeveText { get; set; }
        //[CustomDisplayName("User.OutboundExternal.BasicInfo.DirectedTo")]
        public int? DirectedToId { get; set; }   //صادر الى//

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.Type")]
        public int LetterTypeId { get; set; } //نوع خطاب الصادر//

        //[CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; } //درجة الأسبقية//

        //[CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //درجة السريه//

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.SignedBy")]
        public int? SignedById { get; set; } //موقعة من//

        ////[CustomDisplayName("User.OutboundExternal.BasicInfo.SignedByOrgUnit")]
        //[CustomRequired("User.OutboundExternal.BasicInfo.SignedByOrgUnitRequired")]
        //public int SignedByOrgUnitId { get; set; } 

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.PreparationEntity")]
        [CustomRequired("User.OutboundExternal.BasicInfo.PreparationEntityRequired")]
        public int PreparationEntityId { get; set; }    //الادارة المعدة للصادر//

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.Remarks")]
        public string Remarks { get; set; } //ملاحظات//

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.Subject")]
        [CustomRequired("User.OutboundExternal.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundExternal.BasicInfo.SubjectLength", 500)]
        public string Subject { get; set; } //الموضوع//

        [CustomDisplayName("User.OutboundInternal.BasicInfo.Summary")]
        //[CustomRequired("User.OutboundInternal.BasicInfo.SummaryRequired")]
        [CustomStringLength("User.OutboundInternal.BasicInfo.SummaryLength", 2000, 6)]
        public string Summary { get; set; } //الملخص//

        public string DeliveryMethod { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }
        [CustomStringLength("User.Transaction.Name.PostCodeLength", 12, 0)]
        public string PostCode { get; set; }

        [CustomStringLength("User.Transaction.Name.POBoxLength", 12, 0)]
        public string POBox { get; set; }  //صندوق البريد//
        public bool IsDraft { get; set; }
        public int? ReporterId { get; set; }
        public int? DistrubutionListId { get; set; }
        public int? TransactionPathId { get; set; }
        public int ProcessPeriodTransaction { get; set; }
        public int? SubjectClassificationsId { get; set; }
        public bool isOutboundInternalDraft { get; set; }
        public string ComplaintNumber { get; set; }
        public long Number { get; set; }
        public string ConfidentialityLevelText { get; set; }
        public string CreatedDateH { get; set; }
        public string EntityName { get; set; }
        public int? privacyLevelId { get; set; } //درجة الخصوصية//
        public string LetterNumber { get; set; }
        public bool IsPresentationDraft { get; set; }
        public long? PresentationDraftNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public bool NeedAcknowled { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        public bool IsDecisionDraft { get; set; }
        public bool Encrypted { get; set; }

    }
}