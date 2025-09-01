using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CounterMapping : EntityTypeConfiguration<Counter>
    {
        public CounterMapping()
        {
            this.HasMany(c => c.CounterDetails);            
        }
    }
}
