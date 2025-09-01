namespace MCS.DTO
{
    public class SearchCriteriaBySubjectDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaBySubjectDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO(); 
        }
        public string Subject { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int TypeId { get; set; }
        public int? Year { get; set; }//السنة
        public InboundAdvancedDTO AdvancedSearch { get; set; }
         
    }
}
