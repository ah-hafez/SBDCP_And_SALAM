using Audit.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    [AuditIgnore]
    public class AuditDetails
    {
        public string PropertyName { get; set; }
        public string PropertyOldValue { get; set; }
        public string PropertyNewValue { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
