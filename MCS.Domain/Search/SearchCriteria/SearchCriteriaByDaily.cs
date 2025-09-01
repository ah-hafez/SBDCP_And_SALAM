using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
   public class SearchCriteriaByDaily : BaseSearchCriteria
    {
        public DateTime? TodayDate { get; set; }
        public int OrgUnitId { get; set; }
        public bool HasFullPrivilege { get; set; }
       

    }
}
