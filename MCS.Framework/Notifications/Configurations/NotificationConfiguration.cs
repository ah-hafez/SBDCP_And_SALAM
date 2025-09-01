using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class NotificationConfiguration
    {
        /// <summary>
        /// Gets the section from the config file 
        /// </summary>
        public static NotificationSections GetSections
        {
            get
            {
                return FindSection();
            }
        }

        /// <summary>
        /// Find the Section in the web Configuration file.
        /// </summary>
        private static NotificationSections FindSection()
        {
            try
            {
                NotificationSections configurationSection = 
                    (NotificationSections)System.Configuration.ConfigurationManager.GetSection("NotificationConfiguration/NotificationSections");
                
                if (configurationSection == null)
                {
                    throw new NotificationConfigurationException(String.Format(CultureInfo.InvariantCulture, "AN ERROR OCCURED IN THE NOTIFICATION CONFIGURATION FILE, PLEASE CHECK THAT YOU HAVE A LINK TO THE NOTIFICATIONCONFIGURATION/NOTIFICATIONSECTIONS IN YOUR CONFIG FILE."));
                }

                return configurationSection;
            }
            catch (System.Configuration.ConfigurationException ex)
            {
                throw new NotificationConfigurationException(String.Format(CultureInfo.InvariantCulture, "AN ERROR OCCURED IN THE NOTIFICATION CONFIGURATION FILE, PLEASE CHECK THIS MESSAGE : {0} ", ex.BareMessage), ex);
            }
        }
    }
}
