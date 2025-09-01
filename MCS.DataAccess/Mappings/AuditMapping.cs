using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AuditMapping : EntityTypeConfiguration<MCS.Domain.Audit>
    {
        public AuditMapping()
        {
            this.Property(a => a.EntityName).HasMaxLength(50);
            this.Property(a => a.IPAddress).HasMaxLength(50);
        }
    }
}
