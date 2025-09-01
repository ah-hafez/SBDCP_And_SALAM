using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class InboundAdvancedVM
    {
        [CustomDisplayName("User.InboundAdvancedSearch.ConfedentialityLevel")]
        public int? ConfidentialityId { get; set; }//مستوى السرية

        [CustomDisplayName("User.InboundAdvancedSearch.PriorityLevel")]
        public int? PriorityId { get; set; }//درجة الأسبقية

        [CustomDisplayName("User.InboundAdvancedSearch.TransactionStatus")]
        public int? StatusId { get; set; }//حالة المعاملة   

        [CustomDisplayName("User.InboundAdvancedSearch.LetterType")]
        public int? LetterTypeId { get; set; }//نوع الخطاب الواردة

        [CustomDisplayName("User.InboundAdvancedSearch.SignedBy")]
        public int? SignedById { get; set; }//موقعة من

        [CustomDisplayName("User.InboundAdvancedSearch.InboundFromParty")]
        public int? FromPartyId { get; set; }//الجهة الواردة منها

        [CustomDisplayName("User.InboundAdvancedSearch.Department")]
        public int? SignedByDepartmentId { get; set; }

        [CustomDisplayName("User.InboundAdvancedSearch.SubjectClassifications")]
        public List<int> SubjectClassifications { get; set; }
    }
}