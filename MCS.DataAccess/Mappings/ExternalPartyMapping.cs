using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ExternalPartyMapping : EntityTypeConfiguration<ExternalParty>
    {
        public ExternalPartyMapping()
        {
            HasMany(m => m.PartyManagers).WithRequired(p => p.ExternalParty).WillCascadeOnDelete(true);
            this.Ignore(p => p.LocalName);
            this.Ignore(p => p.LocalAddress);
            this.Ignore(p => p.HasChilds);
            this.Property(e => e.Email).HasMaxLength(50);
            this.Property(e => e.PhoneNumber).HasMaxLength(20);
            this.Property(e => e.Fax).HasMaxLength(20);
            this.Property(e => e.Number).HasMaxLength(20);
        }
    }
}
