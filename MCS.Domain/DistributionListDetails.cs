using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class DistributionListDetails : EntityBase
    {
        public int DistributionListId { get; set; }
        public int? UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public int OrgUnitId { get; set; }        
        public virtual OrgUnit OrgUnit { get; set; }
    }
}
