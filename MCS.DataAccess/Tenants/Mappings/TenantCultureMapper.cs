using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TenantCultureMapper : EntityTypeConfiguration<TenantCulture>
    {
        public TenantCultureMapper()
        {
            this.Property(c => c.ShortName).HasMaxLength(50);
        }
    }
}
