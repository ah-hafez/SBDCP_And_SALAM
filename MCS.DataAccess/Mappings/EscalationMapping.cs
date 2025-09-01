using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess.Mappings
{
    class EscalationMapping : EntityTypeConfiguration<Escalation>
    {
        public EscalationMapping()
        {
            Property(a => a.EscalationActionId).IsRequired();
            Property(a => a.EscalationToId).IsRequired();
            Property(a => a.PriorityId).IsRequired();
        }
    }
}
