using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.AdminAudit
{
    public class AuditLogDTO
    { 
        public string AuditData { get; set; }
        public DateTime AuditDate { get; set; }
        public string AuditAction { get; set; }
        public UserProfileDTO User { get; set; }
        public int AuditUser { get; set; } 
        public string EntityType { get; set; }
        public string GuidId { get; set; }
    }
}
