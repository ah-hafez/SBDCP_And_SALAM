using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Permission : EntityBase
    {
        public string LocalName { get; set; }
        public virtual Lookup Name { get; set; }
        public string Code { get; set; }
        public bool IsUserDefined { get; set; }
        public int? Weight { get; set; }
        public virtual IList<Group> PermissionGroups { get; set; }
    }
}
