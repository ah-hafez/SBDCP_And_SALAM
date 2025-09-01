using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class EditOutboundInternalBasicInfoDTO : BasicInfoBaseDTO
    {
        public long Number { get; set; }  //رقم المعاملة//

        //[CustomDisplayName("User.OutboundInternal.BasicInfo.Source")]
        [CustomRequired("User.OutboundInternal.BasicInfo.SourceRequired")]
        public int TransactionTypeId { get; set; }    //نوع الصادر الداخلي//

        //[CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; } //درجة الأسبقية//

        //[CustomDisplayName("User.OutboundInternal.BasicInfo.Type")]
        [CustomRequired("User.OutboundInternal.BasicInfo.TypeRequired")]
        public int LetterTypeId { get; set; }  //نوع الخطاب الصادر الداخلي//

        //[CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //مستوى السريه//

        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public string ConfidentialityLevelText { get; set; }   //مستوى السريه//



        public string PriorityLeveText { get; set; }


        //[CustomDisplayName("User.OutboundInternal.BasicInfo.Remarks")]
        public string Remarks { get; set; } //ملاحظات//

        //[CustomDisplayName("User.OutboundInternal.BasicInfo.Subject")]
        [CustomRequired("User.OutboundInternal.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundInternal.BasicInfo.SubjectLength", 500)]
        public string Subject { get; set; } //الموضوع//

        //[CustomDisplayName("User.OutboundInternal.BasicInfo.GroupId")]
        public int? GroupId { get; set; }

        public string DeliveryMethod { get; set; }

        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }

        public int? DirectedToId { get; set; }    //موظف المختص //

        [CustomRequired("User.Inbound.BasicInfo.DirectedToOrgUnitRequired")]
        public int DirectedToOrgUnitId { get; set; } //محالة الى
        public int? ReporterId { get; set; }
        public int ProcessPeriodTransaction { get; set; }
        public int? SubjectClassificationsId { get; set; }

        public int? RecordNumber { get; set; }
        public string CreatedDateH { get; set; }
        public string EntityName { get; set; }
        public int? privacyLevelId { get; set; }
        public string LetterNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public bool NeedAcknowled { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        public string Summary { get; set; }
        public bool Encrypted { get; set; }

    }
}