using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class AssignmentUserGroup : EntityBase
    {
        public virtual AssignmentGroup AssignmentGroup { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
