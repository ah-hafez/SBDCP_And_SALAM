using System;

namespace MCS.DTO
{
    public class EditInboundBasicInfoDTO : BasicInfoBaseDTO
    {
        public long InboundNumber { get; set; }  //رقم القيد//
        public string InboundDocumentNumber { get; set; }   //رقم المعاملة الواردة//

        public int TransactionTypeId { get; set; }    //نوع الوارد//

        public int? DestinationId { get; set; }   //جهة الوارد//

        public int? SignedById { get; set; }  //موقعة من//

        public int? SignedByOrgUnitId { get; set; }

        public int? DirectedToId { get; set; }    //موجهة إلى//

        public int DirectedToOrgUnitId { get; set; }
        public int PriorityLevelId { get; set; }  //درجة الأسبقية//

        public int LetterTypeId { get; set; }   //نوع الخطاب الوارد//
        public int ConfidentialityLevelId { get; set; }    //مستوى السريه//

        public string ConfidentialityLevelText { get; set; }    //مستوى السريه//
        public string PriorityLevelText { get; set; }  //درجة الأسبقية//

        public string Remarks { get; set; }  //ملاحظات//
        public string Subject { get; set; }  //الموضوع//
        public string DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
        public int? OutboundDraftId { get; set; }
        public string InboundDateH { get; set; }
        public bool IsForIndividual { get; set; }
        public int? ReporterId { get; set; }
        public string InboundIntendedPerson { get; set; }
        public bool Viewed { get; set; }
        public int? ProcessPeriodTransaction { get; set; }
        public int? SubjectClassificationsId { get; set; }
        public int? RecordNumber { get; set; }
        public int? SideContactExternalEntityID { get; set; }
        public string NumberContact { get; set; }
        public string CreatedDateH { get; set; }
        public string EntityName { get; set; }
        public string ContactDateH { get; set; }
        public int? privacyLevelId { get; set; }
        public string LetterNumber { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string Summary { get; set; }
        public bool Encrypted { get; set; }
        public int? ToUserId { get; set; }
    }
}
