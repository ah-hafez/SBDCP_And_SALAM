using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaBySubjectVM
    {
        public SearchCriteriaBySubjectVM()
        {
            InboundAdvanced = new InboundAdvancedVM();
            OutboundAdvanced = new OutboundAdvancedVM();
        }

        [CustomDisplayName("User.SubjectSearch.Subject")]
        [CustomRequired("User.SubjectSearch.SubjectTypeRequired")]
        public string Subject { get; set; }
        public int TransactionCategory { get; set; }
        public int TransactionTypeId { get; set; }
        [CustomDisplayName("User.OutboundSearch.Year")]
        public int? Year { get; set; }
        public InboundAdvancedVM InboundAdvanced { get; set; }

        public OutboundAdvancedVM OutboundAdvanced { get; set; }
    }
}