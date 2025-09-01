using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class AspNetUserMapping : EntityTypeConfiguration<AspNetUser>
    {
        public AspNetUserMapping()
        {
            HasKey(s => s.Id);
            Property(a => a.Email).HasMaxLength(256);
            Property(a => a.UserName).IsRequired().HasMaxLength(256);
            HasMany(a => a.AspNetRoles);
        }
    }
}
