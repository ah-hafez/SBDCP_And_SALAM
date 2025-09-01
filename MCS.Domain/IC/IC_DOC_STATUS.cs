using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_DOC_STATUS : EntityBase, IAuditable
    {
        public string STATUS_DESC { get; set; }
        public string STATUS_DESC_AR { get; set; }

    }
}
