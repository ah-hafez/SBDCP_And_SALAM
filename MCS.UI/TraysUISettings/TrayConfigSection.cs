using System.Configuration;

namespace MCS.UI.TraysUISettings
{
    public class TrayConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public TrayConfigCollection Trays
        {
            get
            {
                return (TrayConfigCollection)this[""];
            }
            set
            {
                this[""] = value;
            }
        }
    }
}