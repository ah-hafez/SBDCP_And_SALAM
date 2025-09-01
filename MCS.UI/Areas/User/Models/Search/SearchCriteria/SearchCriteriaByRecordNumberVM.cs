using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByRecordNumberVM
    {
        public SearchCriteriaByRecordNumberVM()
        {
            AdvancedSearch = new InboundAdvancedVM();
        }

        [CustomDisplayName("User.DocumentNumberSearch.DocumentNumberSearch")]
        [CustomRequired("User.DocumentNumberSearch.DocumentNumberRequired")]
        [CustomRegularExpressionAttribute("^[0-9ء-ي//\\\\-]*$", "User.Transaction.InboundNumber")]
        public int? RecordNumber { get; set; }

        public InboundAdvancedVM AdvancedSearch { get; set; }
         
    }
}