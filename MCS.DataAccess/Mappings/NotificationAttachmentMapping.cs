using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class NotificationAttachmentMapping : EntityTypeConfiguration<NotificationAttachment>
    {
        public NotificationAttachmentMapping()
        {
            this.Property(l => l.FileName).HasMaxLength(100);
        }
    }
}
