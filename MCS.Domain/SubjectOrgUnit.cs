using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class SubjectOrgUnit : EntityBase
    {
        public int OrgUnitId { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
    }
}
