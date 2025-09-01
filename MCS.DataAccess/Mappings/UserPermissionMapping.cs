using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserPermissionMapping : EntityTypeConfiguration<UserPermission>
    {
        public UserPermissionMapping()
        {
            this.Property(up => up.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(up => new { up.Id, up.UserProfileId, up.PermissionId });
        }
    }
}
