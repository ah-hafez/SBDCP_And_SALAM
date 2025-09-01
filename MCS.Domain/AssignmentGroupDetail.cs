using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class AssignmentGroupDetail : EntityBase
    {
        public virtual UserProfile UserProfile { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
    }
}
