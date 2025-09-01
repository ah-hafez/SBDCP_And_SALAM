using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class PriorityException : EntityBase , IAuditable
    {
        public int PriorityId { get; set; }
        public int OrgUnitId { get; set; }
        public int UserProfileId { get; set; }
        public int LateOnUsersAfter { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public virtual Priority Priority { get; set; }
    }
}
