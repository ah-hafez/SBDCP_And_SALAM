using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class SearchCriteriaByInboundMapper
    {
        public static SearchCriteriaByInboundVM Map(SearchCriteriaByInboundDTO searchCriteriaByInboundDTO)
        {
            if (searchCriteriaByInboundDTO != null)
            {
                SearchCriteriaByInboundVM searchCriteriaByInboundVM = new SearchCriteriaByInboundVM()
                { 
                    AdvancedSearch = InboundAdvancedMapper.Map(searchCriteriaByInboundDTO.AdvancedSearch),
                    DateFrom = searchCriteriaByInboundDTO.DateFrom,
                    DateTo = searchCriteriaByInboundDTO.DateTo,
                    HourFrom = searchCriteriaByInboundDTO.HourFrom,
                    HourTo = searchCriteriaByInboundDTO.HourTo,
                    MinuteFrom = searchCriteriaByInboundDTO.MinuteFrom,
                    MinuteTo = searchCriteriaByInboundDTO.MinuteTo,
                    Number = searchCriteriaByInboundDTO.Number,
                    TimeFrom = searchCriteriaByInboundDTO.TimeFrom,
                    TimeTo = searchCriteriaByInboundDTO.TimeTo,
                    TypeId = searchCriteriaByInboundDTO.TypeId,
                    Year = searchCriteriaByInboundDTO.Year
                };
                return searchCriteriaByInboundVM;
            }
            return new SearchCriteriaByInboundVM();
        }
        public static SearchCriteriaByInboundDTO Map(SearchCriteriaByInboundVM searchCriteriaByInboundVM)
        {
            if (searchCriteriaByInboundVM != null)
            {
                SearchCriteriaByInboundDTO searchCriteriaByInboundDTO = new SearchCriteriaByInboundDTO()
                {
                    AdvancedSearch = InboundAdvancedMapper.Map(searchCriteriaByInboundVM.AdvancedSearch),
                    DateFrom = searchCriteriaByInboundVM.DateFrom,
                    DateTo = searchCriteriaByInboundVM.DateTo,
                    HourFrom = searchCriteriaByInboundVM.HourFrom,
                    HourTo = searchCriteriaByInboundVM.HourTo,
                    MinuteFrom = searchCriteriaByInboundVM.MinuteFrom,
                    MinuteTo = searchCriteriaByInboundVM.MinuteTo,
                    Number = searchCriteriaByInboundVM.Number,
                    TimeFrom = searchCriteriaByInboundVM.TimeFrom,
                    TimeTo = searchCriteriaByInboundVM.TimeTo,
                    TypeId = searchCriteriaByInboundVM.TypeId,
                    Year = searchCriteriaByInboundVM.Year
                };
                return searchCriteriaByInboundDTO;
            }
            return new SearchCriteriaByInboundDTO();
        }
    }
}