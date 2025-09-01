using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ActionMapping : EntityTypeConfiguration<Action>
    {
        public ActionMapping()
        {
            HasRequired(a => a.LocalizationIdentifier).WithMany().WillCascadeOnDelete(false);
            HasRequired(a => a.Type).WithMany().WillCascadeOnDelete(false);
            Ignore(a => a.LocalName);
        }
    }
}
