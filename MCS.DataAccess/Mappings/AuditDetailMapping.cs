using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AuditDetailMapping : EntityTypeConfiguration<AuditDetail>
    {
        public AuditDetailMapping()
        {
            this.Property(a => a.PropertyName).HasMaxLength(100);
            this.Property(a => a.PropertyNewValue).HasMaxLength(1000);
            this.Property(a => a.PropertyOldValue).HasMaxLength(1000);
        }
    }
}
