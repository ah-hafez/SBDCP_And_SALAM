using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Internal
{
    public class EditOutboundInternalBasicInfoVM : BasicInfoBaseVM
    {
        public long Number { get; set; }  //رقم المعاملة//

        //[CustomDisplayName("User.OutboundInternal.BasicInfo.Source")]
        //[CustomRequired("User.OutboundInternal.BasicInfo.SourceRequired")]
        [CustomDisplayName("User.OutboundInternal.BasicInfo.Type")]
        public int TransactionTypeId { get; set; }    //نوع الصادر الداخلي//

        [CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; } //درجة الأسبقية//


        [CustomDisplayName("User.Transaction.BasicInfo.Type")]
        [CustomRequired("User.Transaction.BasicInfo.TypeRequired")]
        public int LetterTypeId { get; set; }  //نوع الخطاب الصادر الداخلي//

        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //مستوى السريه//


        public string ConfidentialityLevelText { get; set; }   //مستوى السريه//

        [CustomDisplayName("User.OutboundInternal.BasicInfo.Remarks")]
        public string Remarks { get; set; } //ملاحظات//

        [CustomDisplayName("User.OutboundInternal.BasicInfo.Subject")]
        [CustomRequired("User.OutboundInternal.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundInternal.BasicInfo.SubjectLength", 2000, 6)]
        public string Subject { get; set; } //الموضوع//
        
        [CustomDisplayName("User.OutboundInternal.BasicInfo.Summary")]
        //[CustomRequired("User.OutboundInternal.BasicInfo.SummaryRequired")]
        [CustomStringLength("User.OutboundInternal.BasicInfo.SummaryLength", 2000, 6)]
        public string Summary { get; set; } //الملخص//

        [CustomDisplayName("User.OutboundInternal.BasicInfo.GroupId")]
        public int? GroupId { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.DeliveryMethod")]
        [CustomRequired("User.Inbound.BasicInfo.DeliveryMethodRequired")]
        public string DeliveryMethod { get; set; }

        [CustomDisplayName("User.Transaction.Assignment.DeliveryMethod")]
        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.DirectedTo")]
        public int? DirectedToId { get; set; }    //موظف المختص //

        [CustomDisplayName("User.Inbound.BasicInfo.DirectedToOrgUnit")]
        [CustomRequired("User.Inbound.BasicInfo.DirectedToOrgUnitRequired")]
        public int DirectedToOrgUnitId { get; set; } //محالة الى

        [CustomDisplayName("User.Transaction.Outbound.Reporter")]
        //[CustomRequired("User.Transaction.ReporterIdRequired")]
        public int? ReporterId { get; set; }

        public int? DistrubutionListId { get; set; }
        public int ProcessPeriodTransaction { get; set; }
        public int? SubjectClassificationsId { get; set; }

        public int? RecordNumber { get; set; }

        public string CreatedDateH { get; set; }
        public string EntityName { get; set; }
        [CustomDisplayName("User.Transaction.PrivecyLevel")]
        public int? privacyLevelId { get; set; }  //مستوى الخصوصية//
        public string LetterNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        [CustomDisplayName("User.OutboundInternal.BasicInfo.Encrypted")]
        [CustomRequired("User.OutboundInternal.BasicInfo.EncryptedRequired")]
        public bool Encrypted { get; set; }

    }
}