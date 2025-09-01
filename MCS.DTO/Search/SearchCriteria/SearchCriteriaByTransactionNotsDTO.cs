using System;

namespace MCS.DTO
{
    public class SearchCriteriaByTransactionNotsDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByTransactionNotsDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO(); 
        }
        public string TransactionNots { get; set; }
        public int TransactionTypeId { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
    }
}