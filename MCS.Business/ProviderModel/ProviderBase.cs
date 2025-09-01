using System.Collections.Specialized;

namespace MCS.Business.ProviderModel
{
    public abstract class ProviderBase
    {
        public abstract void Initialize(string name, NameValueCollection configValue);

        public abstract string Name { get; set; }
    }
}
