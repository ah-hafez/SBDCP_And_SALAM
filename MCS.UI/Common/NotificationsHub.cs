using DocumentFormat.OpenXml.Office2010.Excel;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace MCS.UI.Common
{
    [HubName("NotificationsHub")]
    public class NotificationsHub : Hub
    {
        public static Hashtable Users = new Hashtable();

        public void notifyClient(string name, string msg)
        {
            var connectionId = Users[name];

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            context.Clients.Client(connectionId.ToString()).displayNotification(msg);
        }


        public void onConnected(string name)
        {
            Users.Add(name, Context.ConnectionId);
        }

        public override Task OnConnected()
        {

            var userIdEncrypted = Context.QueryString["userid"];
            var OrgUnitIdEncrypted = Context.QueryString["OrgUnitId"];

            int userId = int.Parse(StringCipher.DecryptStringAES(userIdEncrypted.Replace(" ", "+")));
            int OrgUnitId = int.Parse(StringCipher.DecryptStringAES(OrgUnitIdEncrypted.Replace(" ", "+")));
            var connectionId = Context.ConnectionId;

            var apiUrl = ConfigurationManager.AppSettings["WebApiUrl"] + string.Format("api/Common/AddOnlineUser?userid={0}&OrgUnitId={1}&connectionId={2}", userId, OrgUnitId, connectionId);
            using (var client = new HttpClient())
            {
                var x = client.GetAsync(apiUrl).Result;
            }

            //GetResultExtraData<bool> transactionCertificateDTO = HttpClientWrapper<GetResultExtraData<bool>>.GetItemRequest().Result;

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {

            var connectionId = Context.ConnectionId;

            var apiUrl = ConfigurationManager.AppSettings["WebApiUrl"] + string.Format("api/Common/DeleteOnlineUser?connectionId={0}", connectionId);
            using (var client = new HttpClient())
            {
                var x = client.GetAsync(apiUrl).Result;
            }

            //UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}