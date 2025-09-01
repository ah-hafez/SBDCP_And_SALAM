using Audit.EntityFramework;
using MCS.Framework.Entities;
namespace MCS.Domain
{
    [AuditIgnore]
    public class OnlineUser : EntityBase
    {
        public int UserId { get; set; }
        
        public virtual UserProfile User { get; set; }
        public int? OrgUnitId { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public string ConnectionId { set; get; }
    }
}
