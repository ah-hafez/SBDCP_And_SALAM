using System;
using System.Collections.Generic;
using MCS.Common;
using framework = MCS.Framework.AuditTrail;

namespace MCS.UI
{
    public class AuditVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public DateTime Date { get; set; }
        public framework.OperationType OperationType { get; set; }
        public string OperationTypeName { get; set; }
        public string EntityName { get; set; }
        public List<AuditDetailVM> AuditDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
        public AuditFor AuditFor { get; set; }
    }
}
