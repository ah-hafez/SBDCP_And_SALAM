using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class AllowedAssignmentMapping : EntityTypeConfiguration<AllowedAssignment>
    {

        public AllowedAssignmentMapping()
        {
            this.HasKey(o => o.Id);
      
        }
    }
}
