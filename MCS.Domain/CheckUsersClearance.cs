using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class UsersClearance
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int InboundTransactionsCount { get; set; }
        public int OutboundTransactionsCount { get; set; }
        public int SavedTransactionsCount { get; set; }
    }
}
