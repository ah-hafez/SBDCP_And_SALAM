using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AddOutboundInternalBasicInfoDTO : BasicInfoBaseDTO
    {

        public long Number { get; set; }  //رقم المعاملة//
        
        public int TransactionTypeId { get; set; }    //نوع الصادر الداخلي//
        
        public int PriorityLevelId { get; set; } //درجة الأسبقية//
        public int LetterTypeId { get; set; }  
        public int ConfidentialityLevelId { get; set; }   //مستوى السريه//
        
        public string Remarks { get; set; } //ملاحظات//
        
        public string Subject { get; set; } //الموضوع//
        
        public int? GroupId { get; set; }
        
        public string DeliveryMethod { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; } //طريقة التسليم//
        
        public int? DirectedToId { get; set; }    //موجهة إلى//
        
        public int DirectedToOrgUnitId { get; set; }
        public int? ReporterId { get; set; }
        public int? SubjectClassificationsId { get; set; }

        public int? RecordNumber { get; set; }
        public int? privacyLevelId { get; set; } //درجة الخصوصية//
        public string LetterNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public bool NeedAcknowled { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        public string Summary { get; set; }
        public bool Encrypted { get; set; }

    }
}