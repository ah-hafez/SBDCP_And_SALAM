using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class SearchCriteriaByBarcodeMapper
    {
        public static SearchCriteriaByBarcodeVM Map(SearchCriteriaByBarcodeDTO searchCriteriaByBarcodeDTO)
        {
            if (searchCriteriaByBarcodeDTO != null)
            {
                SearchCriteriaByBarcodeVM searchCriteriaByBarcodeVM = new SearchCriteriaByBarcodeVM()
                { 
                    Barcode = searchCriteriaByBarcodeDTO.Barcode,
                    TypeId = searchCriteriaByBarcodeDTO.TypeId
                };
                return searchCriteriaByBarcodeVM;
            }
            return new SearchCriteriaByBarcodeVM();
        }
        public static SearchCriteriaByBarcodeDTO Map(SearchCriteriaByBarcodeVM searchCriteriaByBarcodeVM)
        {
            if (searchCriteriaByBarcodeVM != null)
            {
                SearchCriteriaByBarcodeDTO searchCriteriaByBarcodeDTO = new SearchCriteriaByBarcodeDTO()
                {
                    Barcode = searchCriteriaByBarcodeVM.Barcode,
                    TypeId = searchCriteriaByBarcodeVM.TypeId
                };
                return searchCriteriaByBarcodeDTO;
            }
            return new SearchCriteriaByBarcodeDTO();
        }
    }
}