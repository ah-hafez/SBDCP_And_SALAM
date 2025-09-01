using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;

namespace MCS.DataAccess
{
    public interface IDocProvidersRepository : IRepository<Domain.DocProviders>
    {
        Domain.DocProviders GetDocProviders(string providerType, int docId, eFileStatus fileStatus);
        Domain.DocProviders GetDocProviders(int id);
        List<Domain.DocProviders> GetDocProviderNotMigrated();
        void Save(Domain.DocProviders docProvider);
        void UpdateDocRepository(Domain.DocProviders docProvider);
        Domain.DocProviders GetDocProviderById(int DocProviderId);
    }
}
