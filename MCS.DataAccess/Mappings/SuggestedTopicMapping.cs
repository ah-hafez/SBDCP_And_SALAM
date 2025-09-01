using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class SuggestedTopicMapping : EntityTypeConfiguration<SuggestedTopic>
    {
        public SuggestedTopicMapping()
        {
            this.Ignore(s => s.IsDeleted);
            this.Ignore(s => s.IsNew);
            this.Ignore(s => s.Text);
            this.Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(s => s.Id);
            this.HasMany(s => s.SubjectOrgUnits).WithOptional().WillCascadeOnDelete();
        }
    }
}
