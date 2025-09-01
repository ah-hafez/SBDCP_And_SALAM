using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionTypeMapping : EntityTypeConfiguration<TransactionType>
    {
        public TransactionTypeMapping()
        {            
            this.Ignore(s => s.Text);
        }
    }
}
