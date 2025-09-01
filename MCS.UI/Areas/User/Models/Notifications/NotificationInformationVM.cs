using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Notifications
{
    public class NotificationInformationVM
    {
        public int TransactionId { get; set; }
        public int TransactionCategory { get; set; }
        public MessageType MessageType { get; set; }
        public string URL { get; set; }
        public string Message { get; set; }
        public string ControllerName { get; set; }
    }
}