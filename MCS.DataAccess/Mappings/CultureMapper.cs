using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CultureMapper : EntityTypeConfiguration<Culture>
    {
        public CultureMapper()
        {
            this.Property(c => c.ShortName).HasMaxLength(50);
        }
    }
}
