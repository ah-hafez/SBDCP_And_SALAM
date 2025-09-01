using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionLinkMapping : EntityTypeConfiguration<TransactionLink>
    {
        public TransactionLinkMapping()
        {
            this.HasRequired(a => a.Type).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.ToTransaction).WithMany().WillCascadeOnDelete(false);
        }
    }
}
