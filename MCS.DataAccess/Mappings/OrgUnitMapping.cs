using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class OrgUnitMapping : EntityTypeConfiguration<OrgUnit>
    {
        public OrgUnitMapping()
        {
            this.HasKey(o => o.Id);
            this.HasMany(o => o.Links).WithOptional().WillCascadeOnDelete();
            this.Ignore(o => o.IsNew);
            this.Ignore(o => o.LocalName);
            this.Ignore(o => o.HasChilds);
            this.HasMany(e => e.Reporters).WithRequired(e => e.OrgUnit).HasForeignKey(e => e.ToEntityId).WillCascadeOnDelete(false);
            this.Property(o => o.BarCode).HasMaxLength(50);
            this.Property(o => o.Number).HasMaxLength(50);
        }
    }
}
