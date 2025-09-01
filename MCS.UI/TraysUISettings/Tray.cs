using System.Configuration;

namespace MCS.UI.TraysUISettings
{
    public class Tray : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public int Id
        {
            get
            {
                return (int)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"].ToString();
            }
            set
            {
                this["name"] = value;
            }
        }


        [ConfigurationProperty("Color", IsRequired = true)]
        public string Color
        {
            get
            {
                return this["Color"].ToString();
            }
            set
            {
                this["Color"] = value;
            }
        }

        [ConfigurationProperty("ImageURL", IsRequired = true)]
        public string ImageURL
        {
            get
            {
                return this["ImageURL"].ToString();
            }
            set
            {
                this["ImageURL"] = value;
            }
        }

        [ConfigurationProperty("css", IsRequired = true)]
        public string Css
        {
            get
            {
                return this["css"].ToString();
            }
            set
            {
                this["css"] = value;
            }
        }

    }
}