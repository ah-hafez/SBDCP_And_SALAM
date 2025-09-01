using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class UserInfo
    {
        public UserInfo()
        {
            ChatWindows = new List<UserChatWindow>();
        }

        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string UserName { get; set; }
        public IList<UserChatWindow> ChatWindows { get; set; }
    }

    public class UserChatWindow
    {
        public int ToUserId { get; set; }
    }
}
