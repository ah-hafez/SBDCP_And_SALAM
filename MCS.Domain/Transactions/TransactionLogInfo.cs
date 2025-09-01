using Audit.EntityFramework;
using System.Collections.Generic;

namespace MCS.Domain
{
    [AuditIgnore]
    public class TransactionLogInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<TransactionLogDetailInfo> TransactionLogDetails { get; set; }
    }
}
