using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class FollowUpDetailsMapping : EntityTypeConfiguration<FollowUpDetails>
    {
        public FollowUpDetailsMapping()
        {
            this.HasRequired(a => a.TransactionFollowUp).WithMany().HasForeignKey(t => t.TransactionFollowUpId).WillCascadeOnDelete(false);
        }
    }
}
