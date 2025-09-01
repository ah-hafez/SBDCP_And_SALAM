using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class HubTransactionMapping : EntityTypeConfiguration<HubTransaction>
    {
        public HubTransactionMapping()
        {
            //HasRequired(a => a.HubAttachments).WithOptional().WillCascadeOnDelete(true);
            //HasRequired(a => a.MainDocument).WithMany().WillCascadeOnDelete(true);

        }
    }
}
