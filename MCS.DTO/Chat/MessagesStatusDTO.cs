using System;

namespace MCS.DTO
{
    public class MessagesStatusDTO
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTimeOffset LastUpdatedDate { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public  ChatRoomDTO Room { get; set; }
        public UserProfileDTO User { get; set; }
        public ChatMessageDTO Message { get; set; }
    }
}
