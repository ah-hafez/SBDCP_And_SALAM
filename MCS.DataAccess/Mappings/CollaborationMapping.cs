using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CollaborationMapping : EntityTypeConfiguration<Collaboration>
    {
        public CollaborationMapping()
        {
            this.Property(t=>t.Text).HasMaxLength(1000);
            this.Property(t => t.DateH).HasMaxLength(20);
        }
    }
}
