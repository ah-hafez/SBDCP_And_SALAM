using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class SearchCriteriaByDocumentNumber : BaseSearchCriteria
    {
        public SearchCriteriaByDocumentNumber()
        {
            AdvancedSearch = new InboundAdvanced();
        }
        public InboundAdvanced AdvancedSearch { get; set; }

        public string DocumentNumber { get; set; }
        public int? Year { get; set; } 
        public int? OrgUnitId { get; set; } 

        public bool HasFullPrivilege { get; set; }
    }
}
