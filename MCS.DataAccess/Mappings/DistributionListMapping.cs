using System.Data.Entity.ModelConfiguration;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class DistributionListMapping : EntityTypeConfiguration<DistributionList>
    {
        public DistributionListMapping()
        {
            HasKey(a => a.Id);
            HasMany(a => a.DistributionListDetails).WithRequired().HasForeignKey(a=>a.DistributionListId)
                .WillCascadeOnDelete(false);

        }
    }
}
