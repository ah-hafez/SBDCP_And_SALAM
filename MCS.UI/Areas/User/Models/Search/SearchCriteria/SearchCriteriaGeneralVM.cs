using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaGeneralVM
    {
        [CustomDisplayName("User.GeneralSearch.Key")]
        [CustomRequired("User.GeneralSearch.KeyRequired")]
        public string Text { get; set; }
    }
}