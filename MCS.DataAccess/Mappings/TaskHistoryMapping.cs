using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TaskHistoryMapping : EntityTypeConfiguration<TaskHistory>
    {

        public TaskHistoryMapping()
        {
            this.Property(t=>t.DateH).HasMaxLength(20);
            this.Property(t => t.DeliveryDateH).HasMaxLength(20);
            this.Property(t => t.StatusDescription).HasMaxLength(500);
          
        }
    }
}
