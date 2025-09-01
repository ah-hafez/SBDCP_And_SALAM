using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    class DocumentAttributeMapping : EntityTypeConfiguration<DocumentAttribute>
    {
        public DocumentAttributeMapping()
        {
            Property(a => a.HijriDate).HasMaxLength(50);
            Property(a => a.Remarks).HasMaxLength(50);
            Property(a => a.DocumentAttributeId).IsRequired();
            Property(a => a.DocumentNumber).IsRequired();
            Property(a => a.DocumentId).IsRequired();
            Property(a => a.Date).IsRequired();

            this.Ignore(s => s.CreatedBy);
            this.Ignore(s => s.CreatedOn);
            this.Ignore(s => s.ModefiedBy);
            this.Ignore(s => s.ModefiedOn);
            this.Ignore(s => s.Id);
        }
    }
}
