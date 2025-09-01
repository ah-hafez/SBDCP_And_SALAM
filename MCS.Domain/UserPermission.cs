using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserPermission : EntityBase, IAuditable
    {
        public int UserProfileId { get; set; }
        public int PermissionId { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
