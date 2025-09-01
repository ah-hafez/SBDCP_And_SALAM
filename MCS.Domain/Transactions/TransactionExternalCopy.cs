
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionExternalCopy : EntityBase, IAuditable
    {
        public int? UserId { get; set; }
        public virtual ExternalPartyManager User { get; set; }
        public int? EntityId { get; set; }
        public virtual ExternalParty Entity { get; set; }
        public int? FromUserId { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public int? FromEntityId { get; set; }
        public virtual OrgUnit FromEntity { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public bool Viewed { get; set; }
        public virtual Action Action { get; set; }
        public int ActionId { get; set; }
        public virtual List<ExternalPartyAttachment> ExternalPartyAttachment { get; set; }
        public int Status { get; set; }
        public bool SendEmail { get; set; }
    }
}

