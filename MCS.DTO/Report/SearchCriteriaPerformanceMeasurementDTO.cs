using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class SearchCriteriaPerformanceMeasurementDTO: BaseReport
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
    }
}
