using System;

namespace MCS.Domain
{
    public class SearchCriteriaByOutboundDraft : BaseSearchCriteria
    {
        public SearchCriteriaByOutboundDraft()
        {
            AdvancedSearch = new OutboundAdvanced();
        }
        public OutboundAdvanced AdvancedSearch { get; set; }
        public int? Number { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int? Year { get; set; }
        public int TypeId { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? OrgUnitId { get; set; } 
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
