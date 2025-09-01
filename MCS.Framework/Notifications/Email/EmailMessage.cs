using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class EmailMessage : NotificationMessage
    {
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public IList<Attachment> Attachments { get; set; }
    }
}
