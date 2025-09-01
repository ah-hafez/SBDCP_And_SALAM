using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserPreferenceMapping : EntityTypeConfiguration<UserPreference>
    {

        public UserPreferenceMapping()
        {
            HasRequired(a => a.UserProfile).WithMany().WillCascadeOnDelete(false);
            //Property(p => p.Email).HasMaxLength(50);
            Ignore(a => a.HasSignaturePasswordText);
        }
    }
}
