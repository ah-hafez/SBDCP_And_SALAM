using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class SearchCriteriaByRecordNumber : BaseSearchCriteria
    {
        public SearchCriteriaByRecordNumber()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced AdvancedSearch { get; set; }

        public int? RecordNumber { get; set; } 
        public int? OrgUnitId { get; set; } 

        public bool HasFullPrivilege { get; set; }
    }
}
