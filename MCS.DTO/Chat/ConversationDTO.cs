using System;
using MCS.Common;

namespace MCS.DTO
{
    public class ConversationChatDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RoomName { get; set; }
        public string Name { get; set; }
        public int? PicId { get; set; }
        public string LastMessage { get; set; }
        public DateTimeOffset When { get; set; }
        public string SendTime { get; set; }
        public int TotalNumberOfUnreadMessages { get; set; }
        public UserStatus Status { get; set; }
    }
}
