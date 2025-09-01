using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class NotificationSections : ConfigurationSection
    {
        /// <summary>
        /// Gets the  Notification section from web.config file.
        /// </summary>
        [ConfigurationProperty("Notification", IsDefaultCollection = false)]
        public NotificationConfigurationSection Notification
        {
            get
            {
                NotificationConfigurationSection notification =
                    (NotificationConfigurationSection)base["Notification"];
                return notification;
            }
        }

        /// <summary>
        /// Gets the  EmailProvidersElement section from web.config file.
        /// </summary>
        [ConfigurationProperty("EmailProvidersElement", IsDefaultCollection = false)]
        public EmailProvidersElement EmailProvidersElement
        {
            get
            {
                EmailProvidersElement emailProvidersElement =
                    (EmailProvidersElement)base["EmailProvidersElement"];
                return emailProvidersElement;
            }
        }
    }
}
