using System;

namespace MCS.DTO
{
    public class SearchCriteriaByManifestNumberDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByManifestNumberDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public int ManifestNumber { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }

    }
}