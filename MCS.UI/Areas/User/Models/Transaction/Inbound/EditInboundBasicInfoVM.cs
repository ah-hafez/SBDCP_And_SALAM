using MCS.Common.CustomAttributes;
using System.Collections.Generic;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Handlers;

namespace MCS.UI.Areas.User.Models.Transaction.Inbound
{
    public class EditInboundBasicInfoVM : BasicInfoBaseVM
    {
        [CustomDisplayName("User.Inbound.BasicInfo.InboundNumber")]
        public long InboundNumber { get; set; }  //رقم القيد//

        [CustomDisplayName("User.Inbound.BasicInfo.InboundDocumentNumber")]
        [CustomRequired("User.Inbound.BasicInfo.InboundDocumentNumberRequired")]
        [CustomStringLength("User.Inbound.BasicInfo.InboundDocumentNumberLength", 40, 0)]
        //[CustomRegularExpressionAttribute("^[0-9ء-ي//\\\\-]*$", "User.Transaction.InboundNumber")]
        public string InboundDocumentNumber { get; set; }   //رقم المعاملة الواردة//

        [CustomDisplayName("User.Inbound.BasicInfo.InboundType")]
        public int TransactionTypeId { get; set; }    //نوع الوارد//

        [CustomDisplayName("User.Inbound.BasicInfo.Destination")]
        public int? DestinationId { get; set; }   //جهة الوارد//

        [CustomDisplayName("User.Inbound.BasicInfo.SignedBy")]
        public int? SignedById { get; set; }  //موقعة من//

        [CustomDisplayName("User.Inbound.BasicInfo.SignedByOrgUnit")]
        public int? SignedByOrgUnitId { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.DirectedTo")]
        [CustomRequired("User.Inbound.BasicInfo.DirectedToRequired")]
        public int? DirectedToId { get; set; }    //موجهة إلى//

        [CustomDisplayName("User.Inbound.BasicInfo.DirectedToOrgUnit")]
        [CustomRequired("User.Inbound.BasicInfo.DirectedToOrgUnitRequired")]
        public int DirectedToOrgUnitId { get; set; }

        [CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityLevelId { get; set; }  //درجة الأسبقية//

        [CustomDisplayName("User.Transaction.BasicInfo.Type")]
        [CustomRequired("User.Transaction.BasicInfo.TypeRequired")]
        public int LetterTypeId { get; set; }   //نوع الخطاب الوارد//

        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }    //مستوى السريه//

        [CustomDisplayName("User.Transaction.Name.City")]
        public int? CityId { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.Remarks")]
        public string Remarks { get; set; }  //ملاحظات//

        [CustomDisplayName("User.Inbound.BasicInfo.Subject")]
        [CustomRequired("User.Inbound.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.Inbound.BasicInfo.SubjectLength", 2000, 6)]

        public string Subject { get; set; }  //الموضوع//

        [CustomDisplayName("User.OutboundInternal.BasicInfo.Summary")]
        //[CustomRequired("User.OutboundInternal.BasicInfo.SummaryRequired")]
        [CustomStringLength("User.OutboundInternal.BasicInfo.SummaryLength", 2000, 6)]
        public string Summary { get; set; } //الملخص//

        [CustomDisplayName("User.Inbound.BasicInfo.DeliveryMethod")]
        [CustomRequired("User.Inbound.BasicInfo.DeliveryMethodRequired")]
        public string DeliveryMethod { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.ReceiveMethod")]
        [CustomRequired("User.Inbound.BasicInfo.DeliveryMethodRequired")]
        public int DeliveryMethodId { get; set; }
        public int? OutboundDraftId { get; set; }
        [CustomDisplayName("User.Inbound.BasicInfo.InboundDateH")]
        [CustomRequired("User.Inbound.InboundDateRequired")]
        public string InboundDateH { get; set; }
        public bool IsForIndividual { get; set; }

        [CustomDisplayName("User.Transaction.Outbound.Reporter")]
        //[CustomRequired("User.Transaction.ReporterIdRequired")]
        public int? ReporterId { get; set; }
        public int? DistrubutionListId { get; set; }
        [CustomDisplayName("User.Inbound.IntendedPerson")]
        public string InboundIntendedPerson { get; set; }
        public int ProcessPeriodTransaction { get; set; }
        public int? SubjectClassificationsId { get; set; }


        public int? RecordNumber { get; set; }
        [CustomDisplayName("User.Inbound.BasicInfo.SideContactExternalEntity")]
        public int? SideContactExternalEntityID { get; set; }
        public string NumberContact { get; set; }

        public string ContactDateH { get; set; }
        [CustomDisplayName("User.Transaction.PrivecyLevel")]
        public int? privacyLevelId { get; set; }  //مستوى الخصوصية//
        public string LetterNumber { get; set; }

        [CustomDisplayName("User.OutboundInternal.BasicInfo.Encrypted")]
        [CustomRequired("User.OutboundInternal.BasicInfo.EncryptedRequired")]
        public bool Encrypted { get; set; }
        public string ConfidentialityLevelText { get; set; }   //مستوى السريه//

    }
}