using System;

namespace MCS.DTO
{
    public class SearchCriteriaByAssignTransactionDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByAssignTransactionDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        //public int UserId { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Number { get; set; }//رقم القيد
        public bool FromEntity { get; set; }
        public int EntityId { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
         
        
    }
}