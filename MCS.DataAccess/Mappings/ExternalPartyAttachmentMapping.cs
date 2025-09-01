using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    class ExternalPartyAttachmentMapping : EntityTypeConfiguration<ExternalPartyAttachment>
    {
        public ExternalPartyAttachmentMapping()
        {
            this.Ignore(e => e.IsDeleted);
            this.HasRequired(a => a.TransactionExternalCopy)
              .WithMany(c => c.ExternalPartyAttachment)
              .HasForeignKey(a => a.TransactionExternalCopyId)
              .WillCascadeOnDelete(false);
            this.HasRequired(a => a.DocumentInfo).WithMany().HasForeignKey(a => a.DocumentInfoId).WillCascadeOnDelete(true);
            this.HasRequired(a => a.ExternalParty).WithMany().HasForeignKey(a => a.PartyId).WillCascadeOnDelete(false);
        }
    }
}
