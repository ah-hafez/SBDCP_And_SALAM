using MCS.Common.CustomAttributes;
using System;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Draft
{
    public class VIPEditOutboundDraftBasicInfoVM : BasicInfoBaseVM
    {
        [CustomDisplayName("User.OutboundDraft.BasicInfo.DraftNumber")]
        public long? DraftNumber { get; set; }    //رقم مشروع الصادر//

        [CustomDisplayName("User.OutboundDraft.BasicInfo.Source")]
        [CustomRequired("User.OutboundDraft.BasicInfo.SourceRequired")]
        public int TransactionTypeId { get; set; }   //نوع الصادر//

        [CustomDisplayName("User.OutboundDraft.BasicInfo.Destination")]
        [CustomRequired("User.OutboundDraft.BasicInfo.DestinationRequired")]
        public int DestinationId { get; set; }    //جهة الصادر//

        [CustomDisplayName("User.OutboundDraft.BasicInfo.DirectedTo")]
        public int? DirectedToId { get; set; }   //موجهة الى//



        [CustomDisplayName("User.OutboundDraft.BasicInfo.SignedBy")]
        public int? SignedById { get; set; } //موقعة من//

        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //مستوى السريه//

        [CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; } //درجة الأسبقية//

        [CustomDisplayName("User.OutboundDraft.BasicInfo.Subject")]
        [CustomRequired("User.OutboundDraft.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundDraft.BasicInfo.SubjectLength", 500)]
        public string Subject { get; set; } //الموضوع//

        [CustomDisplayName("User.Transaction.BasicInfo.Type")]
        public int LetterTypeId { get; set; } //نوع خطاب الصادر//
        [CustomDisplayName("User.OutboundOpen.Source")]
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
        public DateTime CreatedOn { get; set; }

        [CustomDisplayName("User.Transaction.Outbound.Reporter")]
        public int? ReporterId { get; set; }
        public string LetterNumber { get; set; }
    }
}