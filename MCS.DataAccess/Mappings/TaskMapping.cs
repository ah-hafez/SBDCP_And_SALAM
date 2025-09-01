using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TaskMapping : EntityTypeConfiguration<Task>
    {
        public TaskMapping()
        {
            this.HasRequired(a => a.Status).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.ToOrgUnit).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.ToUser).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(a => a.Transaction).WithMany().WillCascadeOnDelete(false);
            this.HasMany(t => t.TasksAttachments).WithRequired(ta => ta.Task).HasForeignKey(ta => ta.TaskId).WillCascadeOnDelete(false);
            this.Property(t=>t.DateH).HasMaxLength(20);
            this.Property(t => t.DeliveryDateH).HasMaxLength(20);
            this.Property(t => t.StatusDescription).HasMaxLength(500);
        }
    }
}
