using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class GroupMapping : EntityTypeConfiguration<Group>
    {
        public GroupMapping()
        {
            this.Ignore(o => o.Name);
        }
    }
}
