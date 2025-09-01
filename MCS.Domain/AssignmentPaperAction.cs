using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class AssignmentPaperAction : EntityBase, IAuditable
    {
        public int ActionId { get; set; }
        public virtual Action Action { get; set; }
    }
}
