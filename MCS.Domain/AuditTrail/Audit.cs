using System;
using System.Collections.Generic;
using Audit.EntityFramework;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class Audit : EntityBase
    {
        public int UserId { get; set; }
        public string IPAddress { get; set; }
        public DateTime Date { get; set; }
        public OperationType OperationType { get; set; }
        public string EntityName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public IList<AuditDetail> Details { get; set; }
        public int TransactionId { get; set; }
    }
}
