using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionAssignmentMapping : EntityTypeConfiguration<TransactionAssignment>
    {
        public TransactionAssignmentMapping()
        {
            this.HasRequired(a => a.FromEntity).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.FromUser).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.PhysicalUser).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.ToEntity).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.Transaction).WithMany(a => a.Assignments).HasForeignKey(t => t.TransactionId).WillCascadeOnDelete(false);
            this.Property(t => t.DateH).HasMaxLength(20);
            this.Ignore(t => t.ReporterId);
        }
    }
}
