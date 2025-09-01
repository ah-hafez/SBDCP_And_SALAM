namespace MCS.DTO
{
    public class SearchCriteriaByRecordNumberDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByRecordNumberDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public int? RecordNumber { get; set; }
        public bool HasFullPrivilege { get; set; }
      
        public InboundAdvancedDTO AdvancedSearch { get; set; }
    }
}
