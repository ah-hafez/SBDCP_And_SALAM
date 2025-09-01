using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class FollowUpDetails : EntityBase
    {
        public string Notes { get; set; }
        public int TransactionFollowUpId { get; set; }
        public virtual TransactionFollowUp TransactionFollowUp { get; set; }
    }
}