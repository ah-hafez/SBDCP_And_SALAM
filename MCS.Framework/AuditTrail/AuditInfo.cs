using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MCS.Framework.AuditTrail
{
    public enum OperationType
    {
        [Description("Enum.OperationType.Insert")]
        Insert = 1,
        [Description("Enum.OperationType.Update")]
        Update = 2,
        [Description("Enum.OperationType.Delete")]
        Delete = 3
    }

    public class AuditInfo
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string IPAddress { get; set; }
        public DateTime Date { get; set; }
        public OperationType OperationType { get; set; }
        public string EntityName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public IList<AuditInfoDetail> Details { get; set; }
        public int TransactionId { get; set; }
    }
}
