using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TenantLookupLocalizationMapping : EntityTypeConfiguration<TenantLookupLocalization>
    {
        public TenantLookupLocalizationMapping()
        {
            this.Property(l => l.Text).HasMaxLength(100);
        }
    }
}
