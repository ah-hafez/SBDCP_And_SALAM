using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class LookupMapping : EntityTypeConfiguration<Lookup>
    {
        public LookupMapping()
        {
            this.Ignore(l => l.Text);
        }
    }
}
