using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TenantLocalizationIdentifierMapping : EntityTypeConfiguration<TenantLocalizationIdentifier>
    {
        public TenantLocalizationIdentifierMapping()
        {
            HasMany(l => l.Localizations)
                .WithRequired(tl => tl.LocalizationIdentifier)
                .HasForeignKey(tl => tl.LocalizationIdentifierId)
                .WillCascadeOnDelete(true);
        }
    }
}
