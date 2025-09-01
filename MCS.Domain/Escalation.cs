using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Escalation : EntityBase
    {
        public int TransactionCategoryId { get; set; }
        public virtual Lookup TransactionCategory { get; set; }
        public int PriorityId { get; set; }
        public virtual Priority Priority { get; set; }
        public int EscalationActionId { get; set; }
        public virtual Lookup EscalationAction { get; set; }
        public int EscalationToId { get; set; }
        public virtual Lookup EscalationTo { get; set; }
        public int EscalationAfterDays { get; set; }

    }
}






