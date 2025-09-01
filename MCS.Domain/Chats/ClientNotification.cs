using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class ClientNotification : EntityBase
    {
        public string Room { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }
        public string Content { get; set; }
    }
}