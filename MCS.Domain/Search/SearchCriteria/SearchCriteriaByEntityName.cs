using System;

namespace MCS.Domain
{
    public class SearchCriteriaByEntityName : BaseSearchCriteria
    {
        public SearchCriteriaByEntityName()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced  AdvancedSearch { get; set; }

        public int? TransactionCategoryId { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int?  OrgUnitId { get; set; }
        public int ExternalPartyId { get; set; }
        public int? Number { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }
}


