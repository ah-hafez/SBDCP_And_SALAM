using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class SMSMessage : NotificationMessage
    {
        public string ToNumber { get; set; }
    }
}
