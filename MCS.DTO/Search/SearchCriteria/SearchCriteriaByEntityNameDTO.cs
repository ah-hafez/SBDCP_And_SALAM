using System;

namespace MCS.DTO
{
    public class SearchCriteriaByEntityNameDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByEntityNameDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO(); 
        }
        public int ExternalPartyId { get; set; }
        public string DocumentNumber { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Number { get; set; }//رقم القيد

        public InboundAdvancedDTO AdvancedSearch { get; set; }
         
    }
}
