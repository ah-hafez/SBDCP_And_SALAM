namespace MCS.Domain
{
    public class SearchCriteriaBySubject : BaseSearchCriteria
    {
        public SearchCriteriaBySubject()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced AdvancedSearch { get; set; }

        public string Subject { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int TypeId { get; set; }
        public int? OrgUnitId { get; set; }
        public int? TransactionCategoryId { get; set; }
        public int? Year { get; set; }//السنة
    }
}

