using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TaskWorkflow : EntityBase
    {
        public virtual OrgUnit FromEntity { get; set; }
        public virtual OrgUnit ToEntity { get; set; }
        public virtual UserProfile ToUser { get; set; }
    }
}
