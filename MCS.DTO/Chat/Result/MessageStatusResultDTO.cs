namespace MCS.DTO
{
    public class MessageStatusResultDTO
  {
        public int UserId { get; set; }
        public int LastReadMessageId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
    }
}
