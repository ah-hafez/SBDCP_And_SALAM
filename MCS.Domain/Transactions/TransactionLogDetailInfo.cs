using Audit.EntityFramework;
using System;

namespace MCS.Domain
{
    [AuditIgnore]
    public class TransactionLogDetailInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
