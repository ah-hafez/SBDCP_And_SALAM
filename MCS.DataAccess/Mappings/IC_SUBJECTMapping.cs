using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    public class IC_SUBJECTMapping : EntityTypeConfiguration<IC_SUBJECT>
    {
        public IC_SUBJECTMapping()
        {
            this.Ignore(p => p.HasChilds);
        }
    }
}
