using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
   public class SearchCriteriaByElcEmployee : BaseSearchCriteria
    {
        public SearchCriteriaByElcEmployee()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced  AdvancedSearch { get; set; }

        public int ElcEmployeeId { get; set; }
        public bool HasFullPrivilege { get; set; } 
        public int? TransactionCategoryId { get; set; }
        public int? OrgUnitId { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

    }
}
