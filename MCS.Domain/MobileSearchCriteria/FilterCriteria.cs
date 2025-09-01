using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.MobileSearchCriteria
{
    public class FilterCriteria
    {
        public int TransNo { get; set; }
        public string Subject { get; set; }
        public DateTime? FromAssignDate { get; set; }
        public DateTime? ToAssignDate { get; set; }
        public int TransSource { get; set; }
    }
}
