using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
   public class SearchCriteriaByExternalOutBoundOrManifestNumber : BaseSearchCriteria
    {
        public SearchCriteriaByExternalOutBoundOrManifestNumber()
        {
            AdvancedSearch = new OutboundAdvanced();
        }
        public OutboundAdvanced  AdvancedSearch { get; set; }

        public int? Number { get; set; }
        public int? Year { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? OrgUnitId { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }
}
