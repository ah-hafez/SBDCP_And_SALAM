using System;

namespace MCS.DTO
{
    public class SearchCriteriaByMilitaryNumberOrIdentityDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByMilitaryNumberOrIdentityDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string IdentificationNumber { get; set; }
        public int TransactionTypeId { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
    }
}