using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_CLASSIFICATION : EntityBase, IAuditable
    {
        public string DESCRIPTION {  get; set; }
        public string DESCRIPTION_AR { get; set; }
        public string LINK_TABLE { get; set; }

    }
}
