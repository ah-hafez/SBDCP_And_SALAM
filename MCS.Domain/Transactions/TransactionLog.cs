using System;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class TransactionLog : EntityBase
    {
        public int UserId { get; set; }
        public UserProfile User { get; set; }
        public Lookup AuditingActionCode { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int TransactionId { get; set; }
    }
}
