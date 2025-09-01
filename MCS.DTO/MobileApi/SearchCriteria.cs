using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class SearchCriteria
    {
        public int TransNo { get; set; }
        public int TransCategory { get; set; }
        public string Subject { get; set; }
        public int EntityId { get; set; }
        public int TransSource { get; set; }
        public DateTime? CreationDateFrom { get; set; }
        public DateTime? CreationDateTo { get; set; }

    }
}