using System.Configuration;

namespace MCS.UI.TraysUISettings
{
    public class TrayConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Tray();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Tray)element).Id;
        }
    }
}