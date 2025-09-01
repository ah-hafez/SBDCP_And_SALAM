using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class ChatMessage : EntityBase
    {
        public string Content { get; set; } 
        public virtual ChatRoom Room { get; set; }
        public virtual UserProfile User { get; set; }
        public DateTimeOffset When { get; set; }
        public bool HtmlEncoded { get; set; }
        public int MessageType { get; set; }

        // After content providers run this is updated with the content
        public string HtmlContent { get; set; }

        public int RoomId { get; set; }
        public int UserId { get; set; }

        // Notifications
        public string ImageUrl { get; set; }
        public string Source { get; set; }
        public virtual ICollection<ChatMessagesStatus> MessagesStatus { get; set; }
        public ChatMessage()
        {
            MessagesStatus = new HashSet<ChatMessagesStatus>();
        }
    }
}