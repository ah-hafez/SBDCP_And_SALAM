using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AttachmentTypeMapping : EntityTypeConfiguration<AttachmentType>
    {
        public AttachmentTypeMapping()
        {
            this.Ignore(l => l.Text);
        }
    }
}
