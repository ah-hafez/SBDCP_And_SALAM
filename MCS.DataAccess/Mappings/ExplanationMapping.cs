using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ExplanationMapping : EntityTypeConfiguration<Explanation>
    {
        public ExplanationMapping()
        {
            this.HasOptional(e => e.Document).WithOptionalDependent().WillCascadeOnDelete();
            this.HasRequired(a => a.Transaction).WithMany(e => e.Explanations).HasForeignKey(t => t.TransactionId).WillCascadeOnDelete(false);
            //this.HasRequired(a => a.Transaction).WithMany().WillCascadeOnDelete(false);
            this.Ignore(p => p.CanBeDeleted);
            this.Ignore(p => p.isCopies);
        }
    }
}
