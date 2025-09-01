using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
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