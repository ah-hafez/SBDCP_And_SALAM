using System.Data.Entity.ModelConfiguration;
using MCS.Domain;


namespace MCS.DataAccess
{
    public class TransactionAssignmentHistoryMapping : EntityTypeConfiguration<TransactionAssignmentHistory>
    {
        public TransactionAssignmentHistoryMapping()
        {
            this.HasRequired(a => a.ToEntity).WithMany().WillCascadeOnDelete(false);
            this.Property(t => t.DateH).HasMaxLength(20);
        }
    }
}
