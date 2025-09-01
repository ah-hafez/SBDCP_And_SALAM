using System;

namespace MCS.DTO
{
     public class ChatClientDTO
    {
        public int Id { get; set; }
        public string UserAgent { get; set; }
        public string Name { get; set; }
        public DateTimeOffset LastActivity { get; set; }
        public DateTimeOffset LastClientActivity { get; set; }
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
