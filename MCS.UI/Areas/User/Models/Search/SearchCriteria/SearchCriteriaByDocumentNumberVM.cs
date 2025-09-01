using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByDocumentNumberVM
    {
        public SearchCriteriaByDocumentNumberVM()
        {
            AdvancedSearch = new InboundAdvancedVM();
        }

        [CustomDisplayName("User.DocumentNumberSearch.DocumentNumberSearch")]
        [CustomRequired("User.DocumentNumberSearch.DocumentNumberRequired")]
        //[CustomRegularExpressionAttribute("^[0-9ء-ي//\\\\-]*$", "User.Transaction.InboundNumber")]
        public string DocumentNumber { get; set; }

        [CustomDisplayName("User.OutboundSearch.Year")]
        public int? Year { get; set; }

        public InboundAdvancedVM AdvancedSearch { get; set; }
         
    }
}