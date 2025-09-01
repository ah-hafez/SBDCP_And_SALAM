using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.External
{
    public class EditOutboundExternalBasicInfoVM : BasicInfoBaseVM
    {
        [CustomDisplayName("User.OutboundExternal.BasicInfo.OutboundNumber")]
        public long OutboundNumber { get; set; } //رقم الصادر//
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Type")]

        public int TransactionTypeId { get; set; }   //نوع الصادر//

        [CustomDisplayName("User.OutboundExternal.BasicInfo.Destination")]
        [CustomRequired("User.OutboundExternal.BasicInfo.DestinationRequired")]
        public int DestinationId { get; set; }    //جهة الصادر//

        [CustomDisplayName("User.OutboundExternal.BasicInfo.DirectedTo")]
        public int? DirectedToId { get; set; }   //صادر الى//
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Destination")]

        public int? ExternalPartyId { get; set; }    //جهة الصادر//

        [CustomDisplayName("User.Transaction.BasicInfo.Type")]
        [CustomRequired("User.Transaction.BasicInfo.TypeRequired")]
        public int LetterTypeId { get; set; } //نوع خطاب الصادر//

        [CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; } //درجة الأسبقية//

        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //درجة السريه//

        [CustomDisplayName("User.OutboundExternal.BasicInfo.SignedBy")]
        public int? SignedById { get; set; } //موقعة من//

        //[CustomDisplayName("User.OutboundExternal.BasicInfo.SignedByOrgUnit")]
        //[CustomRequired("User.OutboundExternal.BasicInfo.SignedByOrgUnitRequired")]
        //public int SignedByOrgUnitId { get; set; } 

        [CustomDisplayName("User.OutboundExternal.BasicInfo.PreparationEntity")]
        //[CustomRequired("User.OutboundExternal.BasicInfo.PreparationEntityRequired")]
        public int? PreparationEntityId { get; set; }    //الادارة المعدة للصادر//

        [CustomDisplayName("User.OutboundExternal.BasicInfo.Remarks")]
        public string Remarks { get; set; } //ملاحظات//

        [CustomDisplayName("User.OutboundExternal.BasicInfo.Subject")]
        [CustomRequired("User.OutboundExternal.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundExternal.BasicInfo.SubjectLength", 2000, 6)]
        public string Subject { get; set; } //الموضوع//

        [CustomDisplayName("User.OutboundInternal.BasicInfo.Summary")]
        //[CustomRequired("User.OutboundInternal.BasicInfo.SummaryRequired")]
        [CustomStringLength("User.OutboundInternal.BasicInfo.SummaryLength", 2000, 6)]
        public string Summary { get; set; } //الملخص//
        public string DeliveryMethod { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        public int DeliveryMethodId { get; set; }
        [CustomDisplayName("User.Transaction.Name.PostCode")]
        [CustomStringLength("User.Transaction.Name.PostCodeLength", 12, 0)]
        public string PostCode { get; set; }

        [CustomDisplayName("User.Transaction.Name.POBox")]
        [CustomStringLength("User.Transaction.Name.POBoxLength", 12, 0)]
        public string POBox { get; set; }  //صندوق البريد//
        [CustomDisplayName("User.OutboundOpen.Source")]
        public bool IsDraft { get; set; }

        [CustomDisplayName("User.Transaction.Outbound.Reporter")]
        public int? ReporterId { get; set; }
        public int? DistrubutionListId { get; set; }
        public int? TransactionPathId { get; set; }
        public int ProcessPeriodTransaction { get; set; }
        public int? SubjectClassificationsId { get; set; }
        public bool isOutboundInternalDraft { get; set; }
        public string ComplaintNumber { get; set; }
        [CustomDisplayName("User.Transaction.PrivecyLevel")]
        public int? privacyLevelId { get; set; }  //مستوى الخصوصية//
        public string LetterNumber { get; set; }
        public bool IsPresentationDraft { get; set; }
        public long? PresentationDraftNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public bool NeedAcknowled { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        public bool IsDecisionDraft { get; set; }
        [CustomDisplayName("User.OutboundInternal.BasicInfo.Encrypted")]
        [CustomRequired("User.OutboundInternal.BasicInfo.EncryptedRequired")]
        public bool Encrypted { get; set; }
        public string ConfidentialityLevelText { get; set; }
    }
}