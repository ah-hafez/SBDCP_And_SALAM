using SolrNet;
using System.Linq;

namespace MCS.Common
{
    public class SolrServiceFactory
    {
        public class Instance<T>
        {
            public void Start()
            {
                var instances = Startup.Container.GetAllInstances(typeof(ISolrOperations<T>));

                if (instances.Count() == 0)
                {
                    Startup.Init<T>(SolrSettings.SolrUrl);
                }
            }
        }
    }
}
