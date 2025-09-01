using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class BarcodeDesignMapping : EntityTypeConfiguration<BarcodeDesign>
    {
        public BarcodeDesignMapping()
        {
            this.Property(b => b.Html).HasMaxLength(4000).IsUnicode(false);
            this.Property(b => b.AttachmentHtml).HasMaxLength(4000).IsUnicode(false);
        }
    }
}
