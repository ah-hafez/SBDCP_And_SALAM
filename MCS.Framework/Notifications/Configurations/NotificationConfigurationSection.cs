using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class NotificationConfigurationSection : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the sender name to be displayed when send message.
        /// </summary>
        [ConfigurationProperty("SMSOriginatingAddress", IsRequired = true)]
        public string SMSOriginatingAddress
        {
            get
            {
                return (string)this["SMSOriginatingAddress"];
            }

            set
            {
                this["SMSOriginatingAddress"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the password to be used in SMS Account.
        /// </summary>
        [ConfigurationProperty("SMSPassword", IsRequired = true)]
        public string SMSPassword
        {
            get
            {
                return (string)this["SMSPassword"];
            }

            set
            {
                this["SMSPassword"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the user name to be used in the SMS account.
        /// </summary>
        [ConfigurationProperty("SMSUserName", IsRequired = true)]
        public string SMSUserName
        {
            get
            {
                return (string)this["SMSUserName"];
            }

            set
            {
                this["SMSUserName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the send SMS url using HTTP protocol.
        /// </summary>
        [ConfigurationProperty("URLToSendHTTPSMS", IsRequired = true)]
        public Uri URLToSendHTTPSMS
        {
            get
            {
                return new Uri(this["URLToSendHTTPSMS"].ToString());
            }

            set
            {
                this["URLToSendHTTPSMS"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the QueryURL.
        /// </summary>
        [ConfigurationProperty("QueryURL", IsRequired = true)]
        public string QueryURL
        {
            get
            {
                return this["QueryURL"].ToString();
            }

            set
            {
                this["QueryURL"] = value;
            }
        }
    }
}
