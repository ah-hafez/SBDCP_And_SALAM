using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditIgnore]
    public class ChatRoom : EntityBase
    {
        public ChatRoom()
        {
            Owners = new HashSet<ChatRoomOwner>();
            Messages = new HashSet<ChatMessage>();
            Users = new HashSet<ChatRoomUser>();
            AllowedUsers = new HashSet<ChatRoomAllowedUser>();
            MessagesStatus = new HashSet<ChatMessagesStatus>();
        }

        public DateTimeOffset? LastNudged { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public bool Closed { get; set; }
        // Private rooms
        public bool Private { get; set; }
        public bool OneToOne { get; set; }
        public int? TransactionId { get; set; }

        public virtual ICollection<ChatRoomAllowedUser> AllowedUsers { get; set; }
        // Creator and owners
        public virtual ICollection<ChatRoomOwner> Owners { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; }
        public virtual ICollection<ChatRoomUser> Users { get; set; }
        public virtual ICollection<ChatMessagesStatus> MessagesStatus { get; set; }
        public virtual Transaction Transaction { get; set; }

    }
}