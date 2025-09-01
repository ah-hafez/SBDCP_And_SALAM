using System.Collections.Generic;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Group : EntityBase, IAuditable
    {
        public string Name { get; set; }
        public bool IsUserDefined { get; set; }
        public bool IsActive { get; set; }
        public virtual Lookup GroupName { get; set; }
        public virtual IList<Permission> Permissions { get; set; }
        public virtual IList<UserGroup> GroupUsers { get; set; }
    }
}
