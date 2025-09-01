using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class AddOutboundExternalBasicInfoVM: BasicInfoBaseVM
    {
        public long OutboundNumber { get; set; } //رقم الصادر//

        public int TransactionTypeId { get; set; }   //نوع الصادر//

        public int? DestinationId { get; set; }    //جهة الصادر//

        public int? DirectedToId { get; set; }   //الشخص المقصود بالصادر//

        public int LetterTypeId { get; set; } //نوع خطاب الصادر//

        public int PriorityLevelId { get; set; } //درجة الأسبقية//

        public int ConfidentialityLevelId { get; set; }   //درجة السريه//

        public int? SignedById { get; set; } //موقعة من//


        public int? PreparationEntityId { get; set; }    //الادارة المعدة للصادر//

        public string Remarks { get; set; } //ملاحظات//

        public string Subject { get; set; } //الموضوع//

        public string PostCode { get; set; }

        public string POBox { get; set; }  //صندوق البريد//

        public string DeliveryMethod { get; set; }

        public int? DeliveryMethodId { get; set; }

        public bool IsDraft { get; set; }
        public int? IsFromDraft { get; set; }

        public int? ReporterId { get; set; }

        public int? DistrubutionListId { get; set; }

        public int? TransactionPathId { get; set; }
        public int? SubjectClassificationsId { get; set; }   //درجة السريه//
        public long BaseTransactionNumber { get; set; }
        public bool isOutboundInternalDraft { get; set; }
        public string ComplaintNumber { get; set; }
    }
}