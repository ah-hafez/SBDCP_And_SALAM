using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class OrgUnitLink : EntityBase
    {
        public virtual OrgUnit FromEntity { get; set; }
        public virtual OrgUnit ToEntity { get; set; }
    }
}
