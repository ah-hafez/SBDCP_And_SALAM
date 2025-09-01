using System;

namespace MCS.Domain
{
    public class SearchCriteriaByExternalPartyCopies : BaseSearchCriteria
    {
        public int? ExternalPartyId { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
