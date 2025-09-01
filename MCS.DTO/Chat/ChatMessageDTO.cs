using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public ChatRoomDTO Room { get; set; }
        public UserProfileDTO User { get; set; }
        public DateTimeOffset When { get; set; }
        public bool HtmlEncoded { get; set; }
        public int MessageType { get; set; }
        // After content providers run this is updated with the content
        public string HtmlContent { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        // Notifications
        public string ImageUrl { get; set; }
        public byte[] UserImage { get; set; }
        public string Source { get; set; }
        public  List<MessagesStatusDTO> MessagesStatus { get; set; }
        public int CreatedBy
        {
            get;
            set;
        }
        public DateTime CreatedOn
        {
            get;
            set;
        }
        public string SendTime { get; set; }
        public string SendDate { get; set; }
    }
}
