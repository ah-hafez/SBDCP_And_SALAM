using System.Collections.Generic;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserCategory : EntityBase, IAuditable
    {
        public virtual LocalizationIdentifier CategoryName { get; set; }
        public string LocalName { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual IList<UserCategoryTray> CategoryTrays { get; set; }
    }
}
