using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class AspNetRoleMapping : EntityTypeConfiguration<AspNetRole>
    {
        public AspNetRoleMapping()
        {
            HasKey(s => s.Id);
            Property(a => a.Name).IsRequired().HasMaxLength(256);
            Property(a => a.Discriminator).IsRequired().HasMaxLength(128);
            HasMany(e => e.AspNetUsers).WithMany(e => e.AspNetRoles)
               .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));
        }
    }
}
