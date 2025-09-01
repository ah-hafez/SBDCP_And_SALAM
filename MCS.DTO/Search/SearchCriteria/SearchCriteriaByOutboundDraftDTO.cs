using System;
using System.Configuration;

namespace MCS.DTO
{
    public class SearchCriteriaByOutboundDraftDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByOutboundDraftDTO()
        {
            AdvancedSearch = new OutboundAdvancedDTO();
        }
        public int? Number { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int? Year { get; set; }
        public int TypeId { get; set; }
     
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public OutboundAdvancedDTO AdvancedSearch { get; set; }
    }
}
