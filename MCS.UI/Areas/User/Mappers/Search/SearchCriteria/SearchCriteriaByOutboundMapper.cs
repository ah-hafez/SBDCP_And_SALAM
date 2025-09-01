using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class SearchCriteriaByOutboundMapper
    {
        public static SearchCriteriaByOutboundVM Map(SearchCriteriaByOutboundDTO searchCriteriaByOutboundDTO)
        {
            if (searchCriteriaByOutboundDTO != null)
            {
                SearchCriteriaByOutboundVM searchCriteriaByOutboundVM = new SearchCriteriaByOutboundVM()
                { 
                    AdvancedSearch = OutboundAdvancedMapper.Map(searchCriteriaByOutboundDTO.AdvancedSearch),
                    DateFrom = searchCriteriaByOutboundDTO.DateFrom,
                    DateTo = searchCriteriaByOutboundDTO.DateTo,
                    HourFrom = searchCriteriaByOutboundDTO.HourFrom,
                    HourTo = searchCriteriaByOutboundDTO.HourTo,
                    MinuteFrom = searchCriteriaByOutboundDTO.MinuteFrom,
                    MinuteTo = searchCriteriaByOutboundDTO.MinuteTo,
                    Number = searchCriteriaByOutboundDTO.Number,
                    TimeFrom = searchCriteriaByOutboundDTO.TimeFrom,
                    TimeTo = searchCriteriaByOutboundDTO.TimeTo,
                    TypeId = searchCriteriaByOutboundDTO.TypeId,
                    Year = searchCriteriaByOutboundDTO.Year
                };
                return searchCriteriaByOutboundVM;
            }
            return new SearchCriteriaByOutboundVM();
        }
        public static SearchCriteriaByOutboundDTO Map(SearchCriteriaByOutboundVM searchCriteriaByOutboundVM)
        {
            if (searchCriteriaByOutboundVM != null)
            {
                SearchCriteriaByOutboundDTO searchCriteriaByOutboundDTO = new SearchCriteriaByOutboundDTO()
                {
                    AdvancedSearch = OutboundAdvancedMapper.Map(searchCriteriaByOutboundVM.AdvancedSearch),
                    DateFrom = searchCriteriaByOutboundVM.DateFrom,
                    DateTo = searchCriteriaByOutboundVM.DateTo,
                    HourFrom = searchCriteriaByOutboundVM.HourFrom,
                    HourTo = searchCriteriaByOutboundVM.HourTo,
                    MinuteFrom = searchCriteriaByOutboundVM.MinuteFrom,
                    MinuteTo = searchCriteriaByOutboundVM.MinuteTo,
                    Number = searchCriteriaByOutboundVM.Number,
                    TimeFrom = searchCriteriaByOutboundVM.TimeFrom,
                    TimeTo = searchCriteriaByOutboundVM.TimeTo,
                    TypeId = searchCriteriaByOutboundVM.TypeId,
                    Year = searchCriteriaByOutboundVM.Year
                };
                return searchCriteriaByOutboundDTO;
            }
            return new SearchCriteriaByOutboundDTO();
        }

    }
}