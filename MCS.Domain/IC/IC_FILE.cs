using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_FILE : EntityBase, IAuditable
    {
        public string FILE_INDEX_NO { get; set; }
        public string FILE_DESC_AR { get; set; }

        public virtual IC_INDEX IcIndex { get; set; }
        public int? IcIndexId { get; set; }
        public int? FILE_USER_ID { get; set; }
        public int? SITE_ID { get; set; }
        public bool ACTIVE { get; set; }
        public int? NUMBER_OF_PARTS { get; set; }
        public string FILE_INDEX_NO_DISPLAY { get; set; }

    }
}
