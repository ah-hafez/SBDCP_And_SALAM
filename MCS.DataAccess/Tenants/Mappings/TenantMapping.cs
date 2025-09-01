using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TenantMapping : EntityTypeConfiguration<Tenant>
    {
        public TenantMapping()
        {
            this.Property(t => t.FromDateH).HasMaxLength(20);
            this.Property(t => t.ToDateH).HasMaxLength(20);
            this.Property(t => t.HostName).HasMaxLength(100);
            this.Property(t => t.DatabaseName).HasMaxLength(100);
            this.Property(t => t.DelegatedUserName).HasMaxLength(50);
            this.Property(t => t.DelegatedEmail).HasMaxLength(50);
            this.Property(t => t.DelegatedMobile).HasMaxLength(20);
            this.Ignore(t => t.LocalName);
            this.Ignore(t => t.LocalDelegatedName);
        }
    }
}
