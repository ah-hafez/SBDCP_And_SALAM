using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserProfileMapping : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMapping()
        {
            HasMany(u => u.Permissions);
            Ignore(u => u.LocalName);
            Property(u => u.Gender);
            Ignore(u => u.Password);
            Property(u => u.UserName).HasMaxLength(50);
            Property(u => u.PhoneNumber).HasMaxLength(20);
            Property(u => u.Email).HasMaxLength(50);
            Property(u => u.IdentityId).HasMaxLength(128);
            Property(u => u.UserNationalId).IsRequired();
        }
    }
}
