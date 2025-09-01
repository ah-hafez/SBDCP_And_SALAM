using MCS.Domain;
using System.Data.Entity.ModelConfiguration;

namespace MCS.DataAccess
{
    public class OnlineUserMapping : EntityTypeConfiguration<OnlineUser>
    {

        public OnlineUserMapping()
        {
            this.HasKey(o => o.Id);
            this.HasRequired(a => a.User).WithMany().WillCascadeOnDelete(false);
        }
    }
}
