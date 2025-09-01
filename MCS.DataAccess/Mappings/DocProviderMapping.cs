using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class DocProviderMapping : EntityTypeConfiguration<DocProviders>
    {
        public DocProviderMapping()
        {
            HasKey(s => s.Id);
            Property(a => a.Provider_Type).IsOptional().HasMaxLength(50);
            Property(a => a.File_Url).IsOptional().HasMaxLength(50);
            Ignore(s => s.CreatedBy);
            Ignore(s => s.CreatedOn);
            Ignore(s => s.ModefiedBy);
            Ignore(s => s.ModefiedOn);
        }
    }
}
