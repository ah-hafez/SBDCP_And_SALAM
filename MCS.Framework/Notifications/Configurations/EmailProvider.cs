using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class EmailProvider : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the EmailProvider class.
        /// </summary>
        public EmailProvider()
        {
        }

        /// <summary>
        /// Gets or sets the Email provider name the distinguish each email provider from other.
        /// </summary>
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return this["Name"].ToString();
            }

            set
            {
                this["Name"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        [ConfigurationProperty("From", IsRequired = true, IsKey = true)]
        public string From
        {
            get
            {
                return this["From"].ToString();
            }

            set
            {
                this["From"] = value;
            }
        }

        /// <summary>
        ///  Gets or sets the user name for the account to send email from.
        /// </summary>
        [ConfigurationProperty("UserName", IsRequired = true, IsKey = true)]
        public string UserName
        {
            get
            {
                return this["UserName"].ToString();
            }

            set
            {
                this["UserName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the password for the account to send  email from.
        /// </summary>
        [ConfigurationProperty("Password", IsRequired = true, IsKey = true)]
        public string Password
        {
            get
            {
                return this["Password"].ToString();
            }

            set
            {
                this["Password"] = value;
            }
        }

        /// <summary>
        /// Gets or sets hot(smtp) to send email Through.
        /// </summary>
        [ConfigurationProperty("Host", IsRequired = true, IsKey = true)]
        public string Host
        {
            get
            {
                return this["Host"].ToString();
            }

            set
            {
                this["Host"] = value;
            }
        }

        /// <summary>
        /// Gets or sets port to send email Through.
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = false, IsKey = true)]
        public string Port
        {
            get
            {
                return this["Port"].ToString();
            }

            set
            {
                this["Port"] = value;
            }
        }

        /// <summary>
        /// Gets or sets  SSl provider from.
        /// </summary>
        [ConfigurationProperty("EnableSSl", IsRequired = true, IsKey = true)]
        public bool EnableSSl
        {
            get
            {
                return(bool) this["EnableSSl"];
            }

            set
            {
                this["EnableSSl"] = value;
            }
        }

    }
}
