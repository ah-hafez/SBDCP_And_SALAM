using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public interface INotificationService
    {
        void Send(NotificationMessage notificationMessage);
    }
}
