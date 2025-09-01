using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_FILE_PARTS : EntityBase, IAuditable
    {
        public virtual IC_FILE File { get; set; }
        public int FileId { get; set; }
        public virtual IC_FILE_ALLOCATION SHELF { get; set; }
        public int? SHELFID { get; set; }
        public int PART_ID { get; set; }
        public DateTime? OPEN_DATE { get; set; }
        public string OPEN_DATE_HJ { get; set; }
        public DateTime? CLOSE_DATE { get; set; }
        public string CLOSE_DATE_HJ { get; set; }
        public string DESCRIPTION_AR { get; set; }
        public short STATUS { get; set; }

    }
}
