using System;

namespace MCS.Domain
{
    public class SearchCriteriaPerformanceMeasurementReport : BaseSearchCriteria
    {
        public int ReportType { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Level { get; set; }
        public int LetterTypeId { get; set; }   
        public bool IsAppointment { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int PriorityLevelId { get; set; } //درجة الاهمية//
        public int ConfidentialityLevelId { get; set; }   //درجة السريه//
        public int TransactionTypeId { get; set; }    
        public string Remarks { get; set; }  //ملاحظات//
        public int DeliveryMethodId { get; set; }//ReceiveId

        public int OrgUnitId { get; set; }
        public int EmployeeId { get; set; }

        public bool? IsPrint { get; set; }
        public int TotalCount { get; set; }
    }
}
