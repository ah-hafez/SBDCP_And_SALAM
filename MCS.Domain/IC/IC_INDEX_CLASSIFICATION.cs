using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_INDEX_CLASSIFICATION : EntityBase, IAuditable
    {
        public virtual IC_INDEX IcIndex { get; set; }
        public int IcIndexId { get; set; }
        public virtual IC_CLASSIFICATION Classification { get; set; }
        public int ClassificationId { get; set; }
        public int Sequance { get; set; }

    }
}
