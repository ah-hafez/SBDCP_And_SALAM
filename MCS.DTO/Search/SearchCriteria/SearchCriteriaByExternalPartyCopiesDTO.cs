using MCS.Common.CustomAttributes;
using System;
using System.Configuration;

namespace MCS.DTO
{
    public class SearchCriteriaByExternalPartyCopiesDTO : BaseSearchCriteriaDTO
    {
        public int? ExternalPartyId { get; set; }
        public bool HasFullPrivilege { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }

    }
}
