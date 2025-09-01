using System;

namespace MCS.DTO
{
    public class SearchCriteriaByTransactionNumberDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByTransactionNumberDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO(); 
        }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int TransactionNumber { get; set; }
        public int TransactionTypeId { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
         
    }
}