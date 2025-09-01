using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class FormMapping : EntityTypeConfiguration<Form>
    {
        public FormMapping()
        {
            this.HasMany(f => f.Departments).WithOptional().HasForeignKey(d => d.FormId).WillCascadeOnDelete();
            this.Ignore(f => f.Text);
        }
    }
}
