using Audit.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace MCS.Domain
{
    [AuditIgnore]
    public class ChatUserPreferences
    {
        //public static ChatUserPreferences GetPreferences(UserProfile chatUser)
        //{
        //    var preferences = chatUser.RawPreferences != null ? JsonConvert.DeserializeObject<ChatUserPreferences>(chatUser.RawPreferences) : new ChatUserPreferences();

        //    // support migrating from versions of preferences with no tabOrder
        //    if (preferences.TabOrder == null)
        //    {
        //        preferences.TabOrder = new List<string> { "Lobby" };
        //        foreach (var room in chatUser.Rooms.Select(e => e.ChatRoom.Name).OrderBy(e => e))
        //        {
        //            preferences.TabOrder.Add(room);
        //        }
        //    }

        //    return preferences;
        //}

        //public void Serialize(UserProfile chatUser)
        //{
        //    chatUser.RawPreferences = JsonConvert.SerializeObject(this);
        //}

        //public IList<string> TabOrder { get; set; }
    }
}
