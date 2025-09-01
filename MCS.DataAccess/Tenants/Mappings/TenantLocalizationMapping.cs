using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TenantLocalizationMapping : EntityTypeConfiguration<TenantLocalization>
    {
        public TenantLocalizationMapping()
        {
            this.Property(l=>l.Text).HasMaxLength(100);
        }
    }
}
