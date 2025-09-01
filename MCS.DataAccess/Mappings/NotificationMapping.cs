using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class NotificationMapping : EntityTypeConfiguration<Notification>
    {
        public NotificationMapping()
        {
            this.Property(l => l.DateH).HasMaxLength(20);
        }
    }
}
