using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class AdvancedSearchMapper
    {
        public static AdvancedSearchVM Map(BaseAdvancedSearchDTO advancedSearchDTO)
        {
            if (advancedSearchDTO != null)
            {
                AdvancedSearchVM advancedSearchVM = new AdvancedSearchVM()
                {
                    BarcodeSearch = SearchCriteriaByBarcodeMapper.Map(advancedSearchDTO.BarcodeSearch),
                    GeneralSearch = SearchCriteriaGeneralMapper.Map(advancedSearchDTO.GeneralSearch),
                    ID = advancedSearchDTO.ID,
                    InboundSearch = SearchCriteriaByInboundMapper.Map(advancedSearchDTO.InboundSearch),
                    OrgUnitId = advancedSearchDTO.OrgUnitId,
                    OutboundSearch = SearchCriteriaByOutboundMapper.Map(advancedSearchDTO.OutboundSearch),
                    SearchTypeId = advancedSearchDTO.SearchTypeId,
                    SubjectSearch = SearchCriteriaBySubjectMapper.Map(advancedSearchDTO.SubjectSearch)
                };
                return advancedSearchVM;
            }
            return new AdvancedSearchVM();
        }
        public static AdvancedSearchDTO Map(AdvancedSearchVM advancedSearchVM)
        {
            if (advancedSearchVM != null)
            {
                AdvancedSearchDTO advancedSearchDTO = new AdvancedSearchDTO()
                {
                    BarcodeSearch = SearchCriteriaByBarcodeMapper.Map(advancedSearchVM.BarcodeSearch),
                    GeneralSearch = SearchCriteriaGeneralMapper.Map(advancedSearchVM.GeneralSearch),
                    ID = advancedSearchVM.ID,
                    InboundSearch = SearchCriteriaByInboundMapper.Map(advancedSearchVM.InboundSearch),
                    OrgUnitId = advancedSearchVM.OrgUnitId,
                    OutboundSearch = SearchCriteriaByOutboundMapper.Map(advancedSearchVM.OutboundSearch),
                    SearchTypeId = advancedSearchVM.SearchTypeId,
                    SubjectSearch = SearchCriteriaBySubjectMapper.Map(advancedSearchVM.SubjectSearch)
                };
                return advancedSearchDTO;
            }
            return new AdvancedSearchDTO();
        }
    }
}