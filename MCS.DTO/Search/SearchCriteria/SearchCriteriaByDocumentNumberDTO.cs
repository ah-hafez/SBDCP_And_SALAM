namespace MCS.DTO
{
    public class SearchCriteriaByDocumentNumberDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByDocumentNumberDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public string DocumentNumber { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int? Year { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
    }
}
