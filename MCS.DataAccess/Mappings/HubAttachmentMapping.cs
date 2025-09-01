using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class HubAttachmentMapping : EntityTypeConfiguration<HubAttachment>
    {
        public HubAttachmentMapping()
        {
            HasRequired(a => a.Type).WithMany().WillCascadeOnDelete(false);
        }
    }
}
