using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class ResourceMapping : EntityTypeConfiguration<Resource>
    {
        public ResourceMapping()
        {
            HasKey(s => s.Id);
            Property(a => a.ResourceId).IsRequired().HasMaxLength(1024);
            Property(a => a.Culture).HasMaxLength(10);
            Property(a => a.ResourceSet).HasMaxLength(512);
            Property(a => a.Type).HasMaxLength(512);
            Property(a => a.Filename).HasMaxLength(128);
            Property(a => a.Comment).HasMaxLength(512);
            Ignore(a => a.CreatedBy);
            Ignore(a => a.CreatedOn);
            Ignore(a => a.ModefiedBy);
            Ignore(a => a.ModefiedOn);
        }
    }
}
