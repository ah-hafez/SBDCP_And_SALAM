using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.EntityFramework;
using MCS.Framework.AuditTrail;

namespace MCS.Domain
{
    [AuditIgnore]
    public class MainAudit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public OperationType OperationType { get; set; }
        public string CreatedBy { get; set; }
    }
}
