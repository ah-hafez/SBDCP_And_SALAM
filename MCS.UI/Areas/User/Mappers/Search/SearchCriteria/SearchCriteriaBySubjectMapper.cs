using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class SearchCriteriaBySubjectMapper
    {
        public static SearchCriteriaBySubjectVM Map(SearchCriteriaBySubjectDTO searchCriteriaBySubjectDTO)
        {
            if (searchCriteriaBySubjectDTO != null)
            {
                SearchCriteriaBySubjectVM searchCriteriaBySubjectVM = new SearchCriteriaBySubjectVM()
                { 
                    InboundAdvanced = InboundAdvancedMapper.Map(searchCriteriaBySubjectDTO.InboundAdvanced),
                    OutboundAdvanced = OutboundAdvancedMapper.Map(searchCriteriaBySubjectDTO.OutboundAdvanced),
                    Subject = searchCriteriaBySubjectDTO.Subject,
                    TypeId = searchCriteriaBySubjectDTO.TypeId
                };
                return searchCriteriaBySubjectVM;
            }
            return new SearchCriteriaBySubjectVM();
        }
        public static SearchCriteriaBySubjectDTO Map(SearchCriteriaBySubjectVM searchCriteriaBySubjectVM)
        {
            if (searchCriteriaBySubjectVM != null)
            {
                SearchCriteriaBySubjectDTO searchCriteriaBySubjectDTO = new SearchCriteriaBySubjectDTO()
                {
                    InboundAdvanced = InboundAdvancedMapper.Map(searchCriteriaBySubjectVM.InboundAdvanced),
                    OutboundAdvanced = OutboundAdvancedMapper.Map(searchCriteriaBySubjectVM.OutboundAdvanced),
                    Subject = searchCriteriaBySubjectVM.Subject,
                    TypeId = searchCriteriaBySubjectVM.TypeId
                };
                return searchCriteriaBySubjectDTO;
            }
            return new SearchCriteriaBySubjectDTO();
        }
    }
}