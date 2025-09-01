using System.Data.Entity.ModelConfiguration;
using MCS.Domain;
namespace MCS.DataAccess.Mappings
{
    class UserGroupMapping : EntityTypeConfiguration<UserGroup>
    {
        public UserGroupMapping()
        {
        }
    }
}
