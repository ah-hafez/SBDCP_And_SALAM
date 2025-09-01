using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_OFFICE : EntityBase, IAuditable
    {

        public string OFFICE_DESC { get; set; }
        public string OFFICE_DESC_AR { get; set; }
        public int ENTITY_ID { get; set; }
        public int? SITE_ID { get; set; }
        public bool DEFAULT_OFFICE { get; set; }


    }
}
