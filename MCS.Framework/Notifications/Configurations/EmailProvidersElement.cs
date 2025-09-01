using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    public class EmailProvidersElement : ConfigurationElement
    {
        /// <summary>
        /// Gets a collection of email providers.
        /// </summary>
        [ConfigurationProperty("EmailProviders", IsDefaultCollection = false)]
        public EmailProviderCollection EmailProviders
        {
            get
            {
                EmailProviderCollection emailProvider = (EmailProviderCollection)base["EmailProviders"];
                return emailProvider;
            }
        }

        /// <summary>
        /// Gets or sets defaultProvider to send email Through.
        /// </summary>
        [ConfigurationProperty("defaultProvider", IsRequired = true, IsKey = true)]
        public string DefaultProvider
        {
            get
            {
                return this["defaultProvider"].ToString();
            }

            set
            {
                this["defaultProvider"] = value;
            }
        }
    }
}
