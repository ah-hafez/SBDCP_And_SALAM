using Audit.EntityFramework;
using System.Collections.Generic;

namespace MCS.Domain
{
    [AuditIgnore]
    public class AllAuditData : MainAudit
    {
        List<AuditDetail> AuditDetails { get; set; }
    }
}
