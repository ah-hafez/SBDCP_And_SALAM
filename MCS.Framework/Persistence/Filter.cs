using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Persistence
{
    public class Filter
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }
        public FilterType Type { get; set; }
    }
}
