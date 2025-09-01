using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class MobileSearchResult
    {
        public int TransID { get; set; }
        public long TransNo { get; set; }
        public string TransTitle { get; set; }
        public string TransDate { get; set; }
        public string TransFrom { get; set; }
        public int TransCategory { get; set; }
        public string FileSize { get; set; }
        public string TransSourceRow { get; set; }
        public string TransNumberRow { get; set; }
        public string EntityName { get; set; }
        public string PrivilegeName { get; set; }
        public bool OutboundDraft { get; set; }
        public bool IsInternalOutbound { get; set; }
    }
}
