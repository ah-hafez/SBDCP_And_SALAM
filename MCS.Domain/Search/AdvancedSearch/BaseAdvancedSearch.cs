using System.Collections.Generic;

namespace MCS.Domain
{
    public class BaseAdvancedSearch
    {
        public int? ConfidentialityId { get; set; }//مستوى السرية

        public int? PriorityId { get; set; }//درجة الأسبقية

        public int? StatusId { get; set; }//حالة المعاملة   

        public int? LetterTypeId { get; set; }//نوع الخطاب الواردة

        public int? SignedById { get; set; }//موقعة من

        public int? FromPartyId { get; set; }//الجهة الواردة منها

        public int? SignedByDepartmentId { get; set; }

        public string SubjectClassifications { get; set; }
        public string DirectedToUserId { get; set; }

        public int? DestinationPartyId { get; set; }//الجهة الصادر اليها

        public int? CreatedDepartmentId { get; set; }//الادارة المنشئة

        public int? DirectedToId { get; set; }


    }
}