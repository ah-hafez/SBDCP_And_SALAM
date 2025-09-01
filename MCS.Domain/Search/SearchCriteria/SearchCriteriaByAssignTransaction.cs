using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
   public class SearchCriteriaByAssignTransaction : BaseSearchCriteria
    {
        public SearchCriteriaByAssignTransaction()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced  AdvancedSearch { get; set; }

        //public int UserId { get; set; }
        public int? TransactionTypeId { get; set; }
        public bool HasFullPrivilege { get; set; }
        public int? OrgUnitId { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool FromEntity { get; set; }
        public int EntityId { get; set; }

    }
}
