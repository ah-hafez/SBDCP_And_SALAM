using System;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    //ToDo: Need to Convert it to cache 
    public class ChatMessagesStatus : EntityBase
    {
        public DateTimeOffset LastUpdatedDate { get; set; } = DateTimeOffset.UtcNow;
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public virtual ChatRoom Room { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual ChatMessage Message { get; set; }
    }
}