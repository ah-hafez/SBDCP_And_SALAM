using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CounterDetailMapping : EntityTypeConfiguration<CounterDetail>
    {
        public CounterDetailMapping()
        {
            //this.HasRequired(d => d.Counter).WithMany(d => d.CounterDetails).HasForeignKey(d => d.CounterId);
        }
    }
}
