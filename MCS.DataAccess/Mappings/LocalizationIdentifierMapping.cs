using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class LocalizationIdentifierMapping : EntityTypeConfiguration<LocalizationIdentifier>
    {
        public LocalizationIdentifierMapping()
        {
            HasMany(l => l.Localizations).WithOptional().WillCascadeOnDelete(true);
        }
    }
}
