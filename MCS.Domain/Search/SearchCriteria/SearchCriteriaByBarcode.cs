namespace MCS.Domain
{
    public class SearchCriteriaByBarcode : BaseSearchCriteria
    {
        public SearchCriteriaByBarcode()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public string Barcode { get; set; }
        public bool HasFullPrivilege { get; set; }
       
        public InboundAdvanced AdvancedSearch { get; set; }

    }
}
