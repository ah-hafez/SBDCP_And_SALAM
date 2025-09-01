using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Notifications;
using MCS.Business;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.Service.Hubs
{
    [HubName("notificationService")]
    public class NotificationService : SignalRNotificationService
    {
        protected readonly ITransactionContextScopeFactory context = IoC.Resolve<ITransactionContextScopeFactory>("transient_TransactionContextScopeFactory");
        private static int connectedClients;

        public System.Threading.Tasks.Task onConnected(int userId, string userName, int orgUnitId)
        {
            if (!Users.Contains(Context.ConnectionId))
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationService>();

                context.Clients.Client(Context.ConnectionId).getOnlineUsers(Users.Values);

                UserInfo userInfo = new UserInfo();

                userInfo.OrgUnitId = orgUnitId;
                userInfo.UserId = userId;
                userInfo.UserName = userName;

                foreach (DictionaryEntry dictionaryEntry in Users)
                {
                    if (((UserInfo)dictionaryEntry.Value).UserId != userId)
                        context.Clients.Client(dictionaryEntry.Key.ToString()).newUserConnected(userInfo);
                }

                Users.Add(Context.ConnectionId, userInfo);

                ++connectedClients;
            }

            return OnConnected();
        }

        public void onSendChatMessage(int toUserId, string message, int? transactionId = null)
        {
            //UserInfo fromUser = (UserInfo)Users[Context.ConnectionId];

            //IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationService>();

            //foreach (DictionaryEntry dictionaryEntry in Users)
            //{
            //    UserInfo toUser = (UserInfo)dictionaryEntry.Value;

            //    if (toUser.UserId == toUserId)
            //    {
            //        context.Clients.Client(dictionaryEntry.Key.ToString()).showMessage(fromUser, message, DateTime.Now.ToString(), transactionId);
            //    }
            //}

            //SaveConversationMessage(fromUser.UserId, toUserId, message, transactionId);

            //context.Clients.Client(Context.ConnectionId).ShowMessageDate(DateTime.Now.ToString());
        }

        //public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        //{
        //    UserInfo userInfo = (UserInfo)Users[Context.ConnectionId];

        //    Users.Remove(Context.ConnectionId);

        //    int sessionConnectecCount = 0;

        //    foreach (object userObject in Users.Values)
        //    {
        //        UserInfo session = (UserInfo)userObject;

        //        if (session.UserId == userInfo.UserId)
        //        {
        //            sessionConnectecCount += 1;
        //        }
        //    }

        //    if (sessionConnectecCount > 0)
        //        return base.OnDisconnected(stopCalled); ;

        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationService>();

        //    foreach (DictionaryEntry dictionaryEntry in Users)
        //    {
        //        context.Clients.Client(dictionaryEntry.Key.ToString()).disconnectUser(userInfo.UserId);
        //    }

        //    return base.OnDisconnected(stopCalled);
        //}

        public void AddChatWindow(int toUserId)
        {
            UserInfo fromUser = (UserInfo)Users[Context.ConnectionId];

            fromUser.ChatWindows.Add(new UserChatWindow() { ToUserId = toUserId });
        }

        public void RemoveChatWindow(int userId)
        {
            UserInfo fromUser = (UserInfo)Users[Context.ConnectionId];

            UserChatWindow userChatWindow = fromUser.ChatWindows.Where(u => u.ToUserId == userId).FirstOrDefault();

            if (userChatWindow != null)
            {
                fromUser.ChatWindows.Remove(userChatWindow);
            }
        }

        private void SaveConversationMessage(int fromUserId, int toUserId, string message, int? transactionId = null)
        {
            using (var transactionContextScope = context.Create())
            {
                Collaboration collaboration = new Collaboration()
                {
                    SenderId = fromUserId,
                    ReceiverId = toUserId,
                    Text = message,
                    TransactionId = transactionId,
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                    Status = CollaborationMessageStatus.Unread,
                };

                if (IsUserOpenChatWindow(toUserId, fromUserId))
                {
                    collaboration.Status = CollaborationMessageStatus.Read;
                }

                ICollaborationBL conversationBL = new CollaborationBL();

                conversationBL.AddCollaboration(collaboration);
            }
        }

        private bool IsUserOpenChatWindow(int toUserId, int fromUserId)
        {
            foreach (DictionaryEntry dictionaryEntry in Users)
            {
                UserInfo toUser = (UserInfo)dictionaryEntry.Value;

                UserChatWindow userChatWindow = toUser.ChatWindows.Where(u => u.ToUserId == fromUserId).FirstOrDefault();

                if (toUser.UserId == toUserId && userChatWindow != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}