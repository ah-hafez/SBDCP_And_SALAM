using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TenantLookupMapping : EntityTypeConfiguration<TenantLookup>
    {
        public TenantLookupMapping()
        {
            this.Ignore(l => l.Text);
        }
    }
}
