using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TaskReminderMapping : EntityTypeConfiguration<TaskReminder>
    {
        public TaskReminderMapping()
        {
            this.Property(t=>t.DateH).HasMaxLength(20);
        }
    }
}
