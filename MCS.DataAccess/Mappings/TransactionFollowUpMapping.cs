using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionFollowUpMapping : EntityTypeConfiguration<TransactionFollowUp>
    {
        public TransactionFollowUpMapping()
        {
            this.HasRequired(a => a.FollowUpUser).WithMany().HasForeignKey(t => t.FollowUpUserId).WillCascadeOnDelete(false);
            this.HasRequired(a => a.FollowUpEntity).WithMany().HasForeignKey(t => t.FollowUpEntityId).WillCascadeOnDelete(false);
        }
    }
}
