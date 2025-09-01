using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class LinkMapping : EntityTypeConfiguration<Link>
    {
        public LinkMapping()
        {
            this.Ignore(l => l.Text);
        }
    }
}
