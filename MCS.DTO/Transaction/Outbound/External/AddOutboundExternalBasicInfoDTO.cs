using System;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AddOutboundExternalBasicInfoDTO : BasicInfoBaseDTO
    {
        public long OutboundNumber { get; set; } //رقم الصادر//

        [CustomRequired("User.OutboundExternal.BasicInfo.TypeRequired")]
        public int TransactionTypeId { get; set; }   //نوع الصادر//

        [CustomRequired("User.OutboundExternal.BasicInfo.DestinationRequired")]
        public int  DestinationId { get; set; }    //جهة الصادر//
        public int? ExternalPartyId { get; set; }    //جهة الصادر الخارجي//
        public int? DirectedToId { get; set; }   //صادر الى//
        public int LetterTypeId { get; set; } //نوع خطاب الصادر//
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; } //درجة الأسبقية//
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //درجة السريه//

        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public string ConfidentialityLevelText { get; set; }   //درجة السريه//


        public int? SignedById { get; set; } //موقعة من//

        [CustomRequired("User.OutboundExternal.BasicInfo.PreparationEntityRequired")]
        public int PreparationEntityId { get; set; }    //الادارة المعدة للصادر//

        public string Remarks { get; set; } //ملاحظات//
        public string Subject { get; set; } //الموضوع//
        public string Summary { get; set; }
        public string PostCode { get; set; }

        public string POBox { get; set; }  //صندوق البريد//
        public string DeliveryMethod { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }
        public bool IsDraft { get; set; }
        public Guid RQUID { get; set; }
        public string OutboundDocumentNumber { get; set; }
        public int? ReporterId { get; set; }
        public int? TransactionPathId { get; set; }
        public int? SubjectClassificationsId { get; set; }   //درجة السريه//
        public bool isOutboundInternalDraft { get; set; }
        public string CreatedDateH { get; set; }
        public string EntityName { get; set; }
        public string ComplaintNumber { get; set; }
        public int? privacyLevelId { get; set; } //درجة الخصوصية//
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