using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_FILE_ALLOCATION : EntityBase, IAuditable
    {
        public int ITEM_NO { get; set; }
        public string ITEM_NAME_AR { get; set; }
        public int PARENT_ID { get; set; }
        public int GROUP_ID { get; set; }
        public int SITE_ID { get; set; }
        public int OFFICE_ID { get; set; }

    }
}
