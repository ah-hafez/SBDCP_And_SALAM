using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApi.Domain
{
    public class IncludedItem
    {
        public int RecordId { get; set; }

        public int ItemId { get; set; }

        public int ItemCount { get; set; }

        public string Desc { get; set; }

        public string Remarks { get; set; }

        public int RowStatus { get; set; }

        public string ItemDate { get; set; }
    }
}
