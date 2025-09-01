using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_FILE_COUNTER : EntityBase, IAuditable
    {
        public int FILE_ID { get; set; }
        public int FILE_COUNTER { get; set; }

    }
}
