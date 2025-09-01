using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Notifications;

namespace MCS.Common.Utility
{
    public static class EmailUtility
    {
       public static bool Send(EmailMessage emailMessage)
        {

            try
            {
                IEmailNotificationService emailNotificationService = new EmailNotificationService();
                emailNotificationService.Send(emailMessage);
                return true;
            }
            catch {
                return false;
            }
               
        }

    }
}
