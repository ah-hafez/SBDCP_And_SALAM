using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
   public class RoomInfoResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Private { get; set; }
        public bool Closed { get; set; }
        public List<UserProfileDTO> Users { get; set; }
        public List<int> Owners { get; set; }
        public List<MessageResultDTO> RecentMessages { get; set; }
        public List<MessageStatusResultDTO> UsersMessageStatus { get; set; }

    }
}
