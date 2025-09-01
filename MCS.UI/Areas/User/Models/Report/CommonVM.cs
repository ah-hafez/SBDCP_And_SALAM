using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class CommonVM
    {
        [CustomDisplayName("User.Transaction.Search.LetterTypeCategory")]
        public int TransactionTypeId { get; set; }   //نوع الصادر//

        [CustomDisplayName("Admin.Priority.HasDate")]
        public bool IsAppointment { get; set; }

        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        public int ConfidentialityLevelId { get; set; }   //درجة السريه//

        [CustomDisplayName("User.Transaction.PriorityLevel")]
        public int PriorityLevelId { get; set; } //درجة الاهمية//

        [CustomDisplayName("User.Transaction.Search.LetterType")]
        public int LetterTypeId { get; set; } //نوع خطاب الصادر//

        [CustomDisplayName("User.Inbound.BasicInfo.Remarks")]
        public string Remarks { get; set; }  //ملاحظات//

        [CustomDisplayName("User.Inbound.BasicInfo.ReceiveMethod")]
        public int ReceiveId { get; set; }
        public DateTime? AppointmentDate { get; set; }

        [CustomDisplayName("User.Transaction.Status")]
        public int TransactionStatusId { get; set; }   //حالة المعامله//
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Type")]
        public int SourceId { get; set; }   //نوع الصادر//
        public int? Hour { get; set; }

        public int? Minute { get; set; }
 
    }
}