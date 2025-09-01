

using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class ChatRoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Private { get; set; }
        public bool OneToOne { get; set; }
        public bool Closed { get; set; }
        public List<ChatRoomUserDTO> Users { get; set; }
        public List<ChatRoomOwnerDTO> Owners { get; set; }
        public List<ChatRoomAllowedUserDTO> AllowedUsers { get; set; }
        public List<ChatMessageDTO> Messages { get; set; }
        public List<MessagesStatusDTO> MessagesStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset? LastNudged { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? TransactionId { get; set; }
        public virtual TransactionDTO Transaction { get; set; }

    }
}
