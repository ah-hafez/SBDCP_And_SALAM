using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class SearchCriteriaGeneralMapper
    {
        public static SearchCriteriaGeneralVM Map(SearchCriteriaGeneralDTO searchCriteriaGeneralDTO)
        {
            if (searchCriteriaGeneralDTO != null)
            {
                SearchCriteriaGeneralVM searchCriteriaGeneralVM = new SearchCriteriaGeneralVM()
                { 
                    Text = searchCriteriaGeneralDTO.Text
                };
                return searchCriteriaGeneralVM;
            }
            return new SearchCriteriaGeneralVM();
        }
        public static SearchCriteriaGeneralDTO Map(SearchCriteriaGeneralVM searchCriteriaGeneralVM)
        {
            if (searchCriteriaGeneralVM != null)
            {
                SearchCriteriaGeneralDTO searchCriteriaGeneralDTO = new SearchCriteriaGeneralDTO()
                {
                    Text = searchCriteriaGeneralVM.Text
                };
                return searchCriteriaGeneralDTO;
            }
            return new SearchCriteriaGeneralDTO();
        }
    }
}