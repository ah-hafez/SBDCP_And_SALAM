using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_INDEX : EntityBase, IAuditable
    {
        public int SEQ { get; set; }
        public string IT_TABLES { get; set; }
        public string IT_DESC { get; set; }
        public string IT_DESC_AR { get; set; }
        public string CAT_CODE { get; set; }
        public string CAT_DISPLAY { get; set; }
        public string CAT_NAME { get; set; }
        public string CAT_NAME_AR { get; set; }
        public bool ACTIVE { get; set; }
        public int INDEX_TYPE { get; set; }

    }
}
