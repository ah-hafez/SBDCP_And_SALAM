using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AttachmentMapping : EntityTypeConfiguration<Attachment>
    {
        public AttachmentMapping()
        {
            //this.HasOptional(a => a.DocumentInfo).WithOptionalDependent().WillCascadeOnDelete();
        }
    }
}
