using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    class AspNetUserClaimMapping : EntityTypeConfiguration<AspNetUserClaim>
    {
        public AspNetUserClaimMapping()
        {
            HasKey(s => s.Id);
            Property(a => a.UserId).IsRequired().HasMaxLength(128);
            Property(a => a.ClaimType).IsOptional();
            Property(a => a.ClaimValue).IsOptional();
            HasRequired(a => a.AspNetUser).WithMany().HasForeignKey(a => a.UserId).WillCascadeOnDelete(false);
            Ignore(s => s.CreatedBy);
            Ignore(s => s.CreatedOn);
            Ignore(s => s.ModefiedBy);
            Ignore(s => s.ModefiedOn);
        }
    }
}