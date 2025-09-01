using System;

namespace MCS.DTO
{
    public class SearchCriteriaByExternalOutBoundOrManifestNumberDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByExternalOutBoundOrManifestNumberDTO()
        {
            AdvancedSearch = new OutboundAdvancedDTO();
        }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Number { get; set; }//رقم القيد
        public int? Year { get; set; }
        public int TransactionTypeId { get; set; }
        public OutboundAdvancedDTO AdvancedSearch { get; set; }
    }
}