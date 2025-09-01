using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class PermissionMapping : EntityTypeConfiguration<Permission>
    {
        public PermissionMapping()
        {
            this.Ignore(o => o.LocalName);
            this.Property(p => p.Code).HasMaxLength(100);
        }
    }
}
