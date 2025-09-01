using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class AuditDetail : EntityBase
    {
        public Audit Audit { get; set; }
        public string PropertyName { get; set; }
        public string PropertyOldValue { get; set; }
        public string PropertyNewValue { get; set; }
    }
}
