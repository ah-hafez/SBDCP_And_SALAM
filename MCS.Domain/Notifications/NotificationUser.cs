using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class NotificationUser : EntityBase
    {
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }
    }
}
