using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.AuditTrail
{
    public class AuditInfoDetail
    {
        public string PropertyName { get; set; }
        public string PropertyOldValue { get; set; }
        public string PropertyNewValue { get; set; }
    }
}
