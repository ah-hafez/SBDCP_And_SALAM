using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using MCS.Framework;
using MCS.Framework.Notifications;

namespace MCS.Business.ASPNETIdentity
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            ISMSNotificationService smsNotificationService = IoC.Resolve<ISMSNotificationService>();

            SMSMessage smsMessage = new SMSMessage
            {
                ToNumber = message.Destination,
                Body = message.Body
            };

            smsNotificationService.Send(smsMessage);

            return Task.FromResult(0);
        }
    }
}
