using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserDelegationMapping : EntityTypeConfiguration<UserDelegation>
    {
        public UserDelegationMapping()
        {
            this.HasRequired(a => a.UserProfile).WithMany().WillCascadeOnDelete(false);
            this.Property(d=>d.FromDateH).HasMaxLength(50);
            this.Property(d => d.ToDateH).HasMaxLength(50);
        }
    }
}
