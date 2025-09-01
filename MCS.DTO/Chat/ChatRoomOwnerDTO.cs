using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
   public class ChatRoomOwnerDTO
    {
        public int RoomId { get; set; }
        public ChatRoomDTO ChatRoom { get; set; }
        public int UserId { get; set; }
        public UserDTO User { get; set; }
    }
}
