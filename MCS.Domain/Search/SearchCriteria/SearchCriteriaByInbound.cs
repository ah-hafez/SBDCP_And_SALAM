using System;

namespace MCS.Domain
{
    public class SearchCriteriaByInbound : BaseSearchCriteria
    {
        public SearchCriteriaByInbound()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced AdvancedSearch { get; set; }
        public int? Number { get; set; }//رقم القيد
        public bool HasFullPrivilege { get; set; }
        public int? Year { get; set; }//السنة
        public int TransactionTypeId { get; set; }
        public int? TransactionCategoryId { get; set; }
        public int? OrgUnitId { get; set; } 
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int DeliveryMethodId { get; set; }
        public string DocumentNumber { get; set; }
    }
}
