using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class UserTenantMapping : EntityTypeConfiguration<UserTenant>
    {
        public UserTenantMapping()
        {
            HasIndex(ut => ut.UserName).IsUnique();
        }
    }
}
