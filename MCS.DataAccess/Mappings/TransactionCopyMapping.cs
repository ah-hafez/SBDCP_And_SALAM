using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionCopyMapping : EntityTypeConfiguration<TransactionCopy>
    {
        public TransactionCopyMapping()
        {
            this.Property(t=>t.DateH).HasMaxLength(20);
        }
    }
}
