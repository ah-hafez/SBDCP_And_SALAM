using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionPathDetailsMapping : EntityTypeConfiguration<TransactionPathDetails>
    {
        public TransactionPathDetailsMapping()
        {
            HasKey(a => a.Id);
            HasRequired(a => a.TransactionPath).WithMany().HasForeignKey(b => b.TransactionPathId).WillCascadeOnDelete(false);
        }
    }
}
