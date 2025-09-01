using System;

namespace MCS.Domain
{
    public class SearchCriteriaByOutboundInternal : BaseSearchCriteria
    {
        public SearchCriteriaByOutboundInternal()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced AdvancedSearch { get; set; }
        public int? Number { get; set; }//رقم القيد
        public bool HasFullPrivilege { get; set; }
        public int? Year { get; set; }//السنة
        public int TypeId { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? OrgUnitId { get; set; } 
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
