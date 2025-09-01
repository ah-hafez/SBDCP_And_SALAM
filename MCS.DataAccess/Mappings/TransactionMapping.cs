using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionMapping : EntityTypeConfiguration<Transaction>
    {
        public TransactionMapping()
        {
            HasKey(o => o.Id);
            HasMany(t => t.Links).WithRequired().HasForeignKey(t => t.TransactionId);
            HasMany(t => t.SubjectClassifications).WithRequired().HasForeignKey(t => t.TransactionId);
            //this.HasMany(t => t.Explanations).WithRequired().HasForeignKey(t => t.TransactionId);
            HasRequired(a => a.Status).WithMany().WillCascadeOnDelete(false);
            HasRequired(a => a.TransactionCategory).WithMany().WillCascadeOnDelete(false);
            HasRequired(a => a.User).WithMany().WillCascadeOnDelete(false);
            HasMany(a => a.FollowUp).WithRequired().HasForeignKey(t => t.TransactionId).WillCascadeOnDelete(false);
            Property(t => t.DateH).HasMaxLength(20);
            Property(t => t.DeliveryReportNumber).HasMaxLength(50);
            Property(t => t.RemindDateH).HasMaxLength(20);
            Property(t => t.DeliveryNumber).HasMaxLength(30);
            Ignore(t => t.GroupId);
            Ignore(t => t.HasPermission);
            Ignore(t => t.TransactionPathId);
            Ignore(t => t.HasLinks);
        }
    }
}
