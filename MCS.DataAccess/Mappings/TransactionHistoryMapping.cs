using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionHistoryMapping : EntityTypeConfiguration<TransactionHistory>
    {
        public TransactionHistoryMapping()
        {
            this.HasRequired(a => a.Transaction).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.TransactionCategory).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.User).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.Status).WithMany().WillCascadeOnDelete(false);
        }
    }
}
