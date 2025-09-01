using System;

namespace MCS.DTO
{
    public class MessageResultDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string HtmlContent { get; set; }
        public DateTimeOffset When { get; set; }
        public UserProfileDTO User { get; set; }
        public byte[] UserImage { get; set; }
        public int MessageType { get; set; }
        public string UserRoomPresence { get; set; }
        public string SendTime { get; set; }
        public string SendDate { get; set; }
    }
}
