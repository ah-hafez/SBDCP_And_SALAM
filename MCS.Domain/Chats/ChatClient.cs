using System;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class ChatClient : EntityBase
    {
        public UserProfile User { get; set; }
        public string UserAgent { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LastActivity { get; set; }
        public DateTimeOffset? LastClientActivity { get; set; }
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}