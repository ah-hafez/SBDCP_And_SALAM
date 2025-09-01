using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class DocumentInfoMapping : EntityTypeConfiguration<DocumentInfo>
    {
        public DocumentInfoMapping()
        {
            this.ToTable("DocumentInfo");
            this.Ignore(d => d.IsDeleted);
            this.Property(d => d.Name).HasMaxLength(200);
            this.Property(d => d.MimeType).HasMaxLength(100);
            this.Property(d => d.ECMId).HasMaxLength(50);
            this.HasOptional(d => d.FromEntity);
            this.HasOptional(d => d.FromUser);
            //this.HasOptional(d => d.Transaction).WithMany().HasForeignKey(x=>x.TransactionId);
        }
    }
}
