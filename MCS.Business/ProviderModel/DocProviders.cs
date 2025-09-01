using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;

namespace MCS.Business.ProviderModel
{
    public class DocProviders
    {
        private MCSDbContext _dbContext = null;
        public Domain.DocProviders GetDocProviders(string providerType, int docId, eFileStatus fileStatus)
        {
            IDocProvidersRepository docProvidersRepository = IoC.Resolve<IDocProvidersRepository>();
            return docProvidersRepository.GetDocProviders(providerType, docId, fileStatus);
        }
        public Domain.DocProviders GetDocProviders(int id)
        {
            IDocProvidersRepository docProvidersRepository = IoC.Resolve<IDocProvidersRepository>();
            return docProvidersRepository.GetDocProviders(id);
        }
        public List<Domain.DocProviders> GetDocProviderNotMigrated()
        {
            IDocProvidersRepository docProvidersRepository = IoC.Resolve<IDocProvidersRepository>();
            return docProvidersRepository.GetDocProviderNotMigrated();
        }

        public void Save(Domain.DocProviders docProvider)
        {
            IDocProvidersRepository docProvidersRepository = IoC.Resolve<IDocProvidersRepository>();
            docProvidersRepository.Save(docProvider);
        }
        public void Update(Domain.DocProviders docProvider)
        {
            IDocProvidersRepository docProvidersRepository = IoC.Resolve<IDocProvidersRepository>();
            docProvidersRepository.UpdateDocRepository(docProvider);
        }
        public Domain.Document GetDocInfo(int id)
        {
            try
            {
                Domain.Document dt = _dbContext.Documents.Where(d => d.Id == id).FirstOrDefault();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDocTransData(int nDocID)
        {
            return null;
        }
        public string[] GetDocType(int nDocID)
        {
            return null;
        }
    }
}
