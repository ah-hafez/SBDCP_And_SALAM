using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class DashboardHome
    {
        public int OutboundCount { get; set; }
        public int OutboundDraftCountCreated { get; set; }
        public int OutboundDraftCountAssigned { get; set; }
        public int InboundCountCreated { get; set; }
        public int InboundCountAssigned { get; set; }
        public int InternalOutboundCountCreated { get; set; }
        public int InternalOutboundCountAssigned { get; set; }
        public int DelayedCount { get; set; }
    }
}
