using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class TransactionEntityDetailMapping : EntityTypeConfiguration<TransactionEntityDetails>
    {
        public TransactionEntityDetailMapping()
        {
            HasKey(s => s.Id);
            HasRequired(a => a.Transaction).WithMany().WillCascadeOnDelete(false);
        }
    }
}
