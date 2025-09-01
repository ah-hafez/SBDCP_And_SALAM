using System;
using System.Configuration;
using System.Xml;

namespace MCS.Business.ProviderModel
{
    public class ProviderConfigurationHandler : IConfigurationSectionHandler
    {
        public virtual object Create(Object parent, Object context, XmlNode node)
        {
            ProviderConfiguration config = new ProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }
    }
}
