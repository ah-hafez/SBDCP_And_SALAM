using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class SearchCriteriaTransactionReportDTO : BaseReport
    {
        public int? Number { get; set; }
        public string Subject { get; set; }
        public int TransactionCategory { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int TransactionTypeId { get; set; }  
        public bool IsAppointment { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int PriorityLevelId { get; set; } //درجة الاهمية//
        public int ConfidentialityLevelId { get; set; }   //درجة السريه//
        public int LetterTypeId { get; set; }                 
        public string Remarks { get; set; }  //ملاحظات//
        public int DeliveryMethodId { get; set; }//ReceiveId
        public int TransactionStatusId { get; set; }   //حالة المعامله//

        public string FullName { get; set; }
        public string CivilID { get; set; }
        public string MobileNumber { get; set; }

        public bool IsForIndividual { get; set; }
        public int DestinationId { get; set; }   //جهة الوارد - ExternalPartyId//
        public string InboundDocumentNumber { get; set; }
        public string InboundDateH { get; set; } //InboundDateH
        public string OutboundDateH { get; set; } //تاريخ الصادر - OutBoundDate 

        public int FromOrgUnitId { get; set; }
        public int ToOrgUnitId { get; set; }
        public int FromEmployeeId { get; set; }
        public int ToEmployeeId { get; set; }
        public int? EntityId { get; set; }
        public int? UserId { get; set; }
        public int Level { get; set; }
        public int SourceId { get; set; }   //نوع الصادر - ExplanationEditorType //
        public int TransactionReportType { get; set; }
    }
}
