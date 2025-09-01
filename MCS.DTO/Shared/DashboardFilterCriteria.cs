using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class DashboardFilterCriteria
    {
        public int level { get; set; }
        public int entityId { get; set; }
        public int userId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int itemId { get; set; }
        public string cultureId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
