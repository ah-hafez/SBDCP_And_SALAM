using System.Collections;
using System.Collections.Specialized;
using System.Xml;

namespace MCS.Business.ProviderModel
{
    public class ProviderConfiguration
    {
        string defaultProvider;
        Hashtable providers = new Hashtable();

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            XmlAttributeCollection attributeCollection = node.Attributes;

            // Get the default provider
            defaultProvider = attributeCollection["defaultProvider"].Value;

            // Read child nodes
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "providers")
                {
                    GetProviders(child);
                }
            }
        }

        void GetProviders(XmlNode node)
        {
            foreach (XmlNode provider in node.ChildNodes)
            {
                switch (provider.Name)
                {
                    case "add":
                        providers.Add(provider.Attributes["name"].Value, new Provider(provider.Attributes));
                        break;

                    case "remove":
                        providers.Remove(provider.Attributes["name"].Value);
                        break;

                    case "clear":
                        providers.Clear();
                        break;
                }
            }
        }

        public string DefaultProvider { get { return defaultProvider; } }

        public Hashtable Providers { get { return providers; } }
    }
    public class Provider
    {
        private string name;
        private string providerType;
        private string providerAssembly;

        NameValueCollection providerAttributes = new NameValueCollection();

        public Provider(XmlAttributeCollection attributes)
        {
            // Set the name of the provider
            name = attributes["name"].Value;

            // Set the type of the provider
            string[] configType = attributes["type"].Value.Split(',');
            if (configType.Length >= 2)
            {
                providerType = configType[0];
                providerAssembly = configType[1];
            }

            // Store all the attributes in the attributes bucket
            foreach (XmlAttribute attribute in attributes)
            {
                if ((attribute.Name != "name") && (attribute.Name != "type"))
                {
                    providerAttributes.Add(attribute.Name, attribute.Value);
                }
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Type
        {
            get
            {
                return providerType;
            }
        }

        public string AssemblyName
        {
            get
            {
                return providerAssembly;
            }
        }

        public NameValueCollection Attributes
        {
            get
            {
                return providerAttributes;
            }
        }
    }
}
