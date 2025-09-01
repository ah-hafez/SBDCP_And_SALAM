using Audit.EntityFramework;
using MCS.Common;

namespace MCS.Domain
{
    [AuditIgnore]
    public class AuditingInfo
    {
        public AuditingActionCode AuditingActionCode { get; set; }
    }
}
