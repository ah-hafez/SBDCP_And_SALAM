using System;
using System.Configuration;

namespace MCS.DTO
{
    public class SearchCriteriaByOutboundInternalDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByOutboundInternalDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public int? Number { get; set; }//رقم القيد
        public bool HasFullPrivilege { get; set; }
        public int? Year { get; set; }//السنة
        public int TypeId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }

    }
}
