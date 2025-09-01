using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class SearchCriteriaTransactionReport : BaseSearchCriteria
    {
        //Basic Search
        public int? Number { get; set; }
        public string Subject { get; set; }
        public int TransactionCategoryId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        //Common
        public int TransactionTypeId { get; set; }   //نوع خطاب الصادر - ExplanationEditorType //
        public bool IsAppointment { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int PriorityLevelId { get; set; } //درجة الاهمية//
        public int ConfidentialityLevelId { get; set; }   //درجة السريه//
        public int LetterTypeId { get; set; } // SubjectClassifications//                
        public string Remarks { get; set; }  //ملاحظات//
        public int DeliveryMethodId { get; set; }//ReceiveId
        public int TransactionStatusId { get; set; }

        //Names
        public string FullName { get; set; }
        public string CivilID { get; set; }
        public string MobileNumber { get; set; }

        //Additional Fields
        public bool IsForIndividual { get; set; }
        public int DestinationId { get; set; }   //جهة الوارد - ExternalPartyId//
        public string InboundDocumentNumber { get; set; }
        public string InboundDateH { get; set; } //InboundDateH
        public string OutboundDateH { get; set; } //تاريخ الصادر - OutBoundDate 

        //Assignment
        public int FromOrgUnitId { get; set; }
        public int ToOrgUnitId { get; set; }
        public int FromEmployeeId { get; set; }
        public int ToEmployeeId { get; set; }

        public bool? IsPrint { get; set; }
        public int TotalCount { get; set; }
        public int? EntityId { get; set; }
        public int? UserId { get; set; }
        public int Level { get; set; }
    }
}
