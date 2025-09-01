using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionPathMapping : EntityTypeConfiguration<TransactionPath>
    {
        public TransactionPathMapping()
        {
            HasKey(a => a.Id);
            HasMany(a => a.TransactionPathDetails).WithRequired().HasForeignKey(a=>a.TransactionPathId).WillCascadeOnDelete(false);

        }
    }
}
