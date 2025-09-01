using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AssignmentGroupMapping : EntityTypeConfiguration<AssignmentGroup>
    {
        public AssignmentGroupMapping()
        {
            this.Ignore(o => o.LocalName);
        }
    }
}
