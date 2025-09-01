using System;

namespace MCS.DTO
{
    public class SearchCriteriaByElcEmployeeDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByElcEmployeeDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO(); 
        }
        public int ElcEmployeeId { get; set; }
         public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int TransactionCategory { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
         
    }
}