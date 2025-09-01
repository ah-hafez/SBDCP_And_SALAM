using MCS.Common;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;

namespace MCS.Domain
{
    public class TransactionEncryptionCode : EntityBase, IAuditable
    {

        public string Code { get; set; }
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public UserProfile User { get; set; }
        public int OrgUnitId { get; set; }
        public OrgUnit OrgUnit { get; set; }
        public DateTime CodeExpireDate { get; set; }
        public EncryptionChannel EncryptionChannel { get; set; }
    }
}
