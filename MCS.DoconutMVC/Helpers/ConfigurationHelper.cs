using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MCS.DoconutMVC.Helpers
{
    public class ConfigurationHelper : ConfigurationSection
    {
        // Create and return a "basepath" attribute.
        [ConfigurationProperty("BasePath", DefaultValue = "/")]
        public string BasePath
        {
            get
            {
                return (string)this["BasePath"];
            }
        }
    }
}