using Audit.EntityFramework;
using MCS.Framework.Entities;
using System;

namespace MCS.Domain
{
    [AuditIgnore]
    public class FollowUpAuditTrail : EntityBase
    {

        public int FollowupId { get; set; }
        public int ProccessId { get; set; }
        public string ProccessDescription { get; set; }
        public DateTime ProccessDate { get; set; }
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public int EntityId { get; set; }
        public virtual OrgUnit Entity { get; set; }
    }
}
