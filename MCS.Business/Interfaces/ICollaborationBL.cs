using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface ICollaborationBL
    {
        void AddCollaboration(Collaboration collaboration);
        bool HasCollaboration(int toUserId, int transactionId);
        ChatNotificationsInfo GetChatNotifications();
        IList<CollaborationUserInfo> GetAllCollaborationUsers(string cultureName);
        IList<Collaboration> GetCollaboration(int toUserId, int pageSize, string cultureName);
        IList<Collaboration> GetCollaboration(int toUserId, int pageSize, int startId, string cultureName);
      
    }
}
