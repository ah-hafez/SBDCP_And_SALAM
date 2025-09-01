using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class LocalizationMapping : EntityTypeConfiguration<Localization>
    {
        public LocalizationMapping()
        {
            this.Property(l=>l.Text).HasMaxLength(1000);
        }
    }
}
