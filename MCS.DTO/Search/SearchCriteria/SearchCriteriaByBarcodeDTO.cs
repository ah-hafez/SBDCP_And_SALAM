namespace MCS.DTO
{
    public class SearchCriteriaByBarcodeDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByBarcodeDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public string Barcode { get; set; }
        public bool HasFullPrivilege { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
    }
}
