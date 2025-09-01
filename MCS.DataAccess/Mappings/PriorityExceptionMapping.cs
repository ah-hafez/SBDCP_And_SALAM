using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    class PriorityExceptionMapping : EntityTypeConfiguration<PriorityException>
    {
        public PriorityExceptionMapping()
        {
            this.HasKey(model => model.Id);
        }
    }
}
