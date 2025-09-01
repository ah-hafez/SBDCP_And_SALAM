using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class OutboundDraftAdvancedVM
    {
        [CustomDisplayName("User.OutboundAdvancedSearch.DestinationParty")]
        public int? DestinationPartyId { get; set; }//الجهة الصادر اليها

        [CustomDisplayName("User.OutboundAdvancedSearch.CreatedDepartment")]
        public int? CreatedDepartmentId { get; set; }//الادارة المنشئة

        [CustomDisplayName("User.OutboundAdvancedSearch.DirectedTo")]
        public int? DirectedToId { get; set; }

        [CustomDisplayName("User.OutboundAdvancedSearch.ConfedentialityLevel")]
        public int? ConfidentialityId { get; set; }//مستوى السرية

        [CustomDisplayName("User.OutboundAdvancedSearch.PriorityLevel")]
        public int? PriorityId { get; set; }//درجة الأسبقية

        [CustomDisplayName("User.OutboundAdvancedSearch.TransactionStatus")]
        public int? StatusId { get; set; }//حالة المعاملة   

        [CustomDisplayName("User.OutboundAdvancedSearch.SubjectClassifications")]
        public List<int> SubjectClassifications { get; set; }
    }
}