using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class AspNetUserLoginMapping : EntityTypeConfiguration<AspNetUserLogin>
    {
        public AspNetUserLoginMapping()
        {
            HasKey(a => new { a.LoginProvider, a.ProviderKey, a.UserId });
            Property(a => a.LoginProvider).IsRequired().HasColumnOrder(0);
            Property(a => a.ProviderKey).IsRequired().HasColumnOrder(1);
            Property(a => a.UserId).IsRequired().HasColumnOrder(2);
            HasRequired(a => a.AspNetUser).WithMany().HasForeignKey(a => a.UserId).WillCascadeOnDelete(false);
        }
    }
}
