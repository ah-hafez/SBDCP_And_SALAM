using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class PriorityMapping : EntityTypeConfiguration<Priority>
    {
        public PriorityMapping()
        {                        
            this.Ignore(p => p.Text);
        }
    }
}
