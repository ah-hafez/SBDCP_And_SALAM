using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class BaseReport
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool Ascending { get; set; }
        public string CultureName { get; set; }
        public string OrderBy { get; set; }
        public bool? IsPrint { get; set; }
        public int TotalCount { get; set; }
    }
}
