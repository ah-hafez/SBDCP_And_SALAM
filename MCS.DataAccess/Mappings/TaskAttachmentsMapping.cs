using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using MCS.Domain;


namespace MCS.DataAccess
{
    public class TaskAttachmentsMapping : EntityTypeConfiguration<TasksAttachments>
    {
        public TaskAttachmentsMapping()
        {
            this.HasRequired(a => a.Task).WithMany().HasForeignKey(a => a.TaskId).WillCascadeOnDelete(false);
            this.HasRequired(a => a.DocumentInfo).WithMany().HasForeignKey(a => a.DocumentInfoId).WillCascadeOnDelete(false);
        }
    }
}
