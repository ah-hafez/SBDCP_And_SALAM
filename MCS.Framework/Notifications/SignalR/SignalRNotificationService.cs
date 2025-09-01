using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MCS.Framework.Notifications
{
    //TODD:Check Code Design Width Asfour (UserInfo Class  , orgUnitId in connected Method  and CultureName )
    [HubName("signalRNotificationService")]
    public class SignalRNotificationService : Hub, ISignalRNotificationService
    {
        private static int connectedClients;

        public static Hashtable Users = new Hashtable();

        public override Task OnDisconnected(bool stopCalled)
        {
            UserInfo userInfo = (UserInfo)Users[Context.ConnectionId];
            Users.Remove(Context.ConnectionId);

            int sessionConnectecCount = 0;

            foreach (object userObject in Users.Values)
            {
                UserInfo session = (UserInfo)userObject;
                if (userInfo!=null&&session.UserId == userInfo.UserId)
                {
                    sessionConnectecCount += 1;
                }

            }

            if (sessionConnectecCount > 0)
                return base.OnDisconnected(stopCalled); ;


            IHubContext context =
               GlobalHost.ConnectionManager.GetHubContext<SignalRNotificationService>();

            foreach (DictionaryEntry dictionaryEntry in Users)
            {
                if (userInfo != null)
                {
                    context.Clients.Client(dictionaryEntry.Key.ToString()).disconnectUser(userInfo.UserId); 
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public void Send(NotificationMessage notificationMessage)
        {
            if (notificationMessage == null)
            {
                throw new ArgumentException("Notification Message Cannot be Null");
            }

            SignalRMessage signalRMessage = notificationMessage as SignalRMessage;

            IHubContext hubContext =
                GlobalHost.ConnectionManager.GetHubContext<SignalRNotificationService>();

            hubContext.Clients.All.ShowMessage(notificationMessage.Body);
        }

        public void SendToUser(int userId, NotificationMessage notificationMessage)
        {
            if (notificationMessage == null)
            {
                throw new ArgumentException("Notification Message Cannot be Null");
            }

            SignalRMessage signalRMessage = notificationMessage as SignalRMessage;

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SignalRNotificationService>();

            foreach (DictionaryEntry dictionaryEntry in Users)
            {
                UserInfo userInfo = (UserInfo)dictionaryEntry.Value;

                if (userInfo.UserId == userId)
                {
                    context.Clients.Client(dictionaryEntry.Key.ToString()).displayNotification(notificationMessage);
                }
            }
        }
    }
}