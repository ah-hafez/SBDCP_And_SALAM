using System;
using System.Collections.Generic;
using MCS.Framework.AuditTrail;

namespace MCS.DTO
{
    public class AuditDTO 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public DateTime Date { get; set; }
        public OperationType OperationType { get; set; }
        public string EntityName { get; set; }
        public List<AuditDetailDTO> AuditDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}
