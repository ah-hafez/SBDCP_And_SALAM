using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.DataAccess;

namespace MCS.DataAccess
{
    public class DocProvidersRepository : BaseRepository<Domain.DocProviders>, IDocProvidersRepository
    {
        public DocProvidersRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }
        public Domain.DocProviders GetDocProviders(string providerType, int docId, eFileStatus fileStatus)
        {
            try
            {
                Domain.DocProviders docProviders = _oMCSDbContext.DocProviders.Where(doc => doc.File_Doc_Id == docId && doc.Provider_Type == providerType && doc.File_Status == fileStatus).FirstOrDefault();
                Domain.DocProviders docProvider = new Domain.DocProviders()
                {
                    File_Id = docProviders.File_Id,
                    File_Doc_Id = docProviders.File_Doc_Id,
                    Id = docProviders.Id,
                    File_Is_Migrated = docProviders.File_Is_Migrated,
                    File_Status = docProviders.File_Status,
                    File_Url = docProviders.File_Url,
                    Provider_Type = docProviders.Provider_Type,
                    TRANS_ID = docProviders.TRANS_ID
                };
                return docProvider;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Domain.DocProviders GetDocProviders(int id)
        {
            try
            {
                Domain.DocProviders docProviders= _oMCSDbContext.DocProviders.Where(doc => doc.File_Doc_Id == id).FirstOrDefault();
                Domain.DocProviders docProvider = new Domain.DocProviders()
                {
                    File_Id = docProviders.File_Id,
                    File_Doc_Id = docProviders.File_Doc_Id,
                    Id = docProviders.Id,
                    File_Is_Migrated = docProviders.File_Is_Migrated,
                    File_Status = docProviders.File_Status,
                    File_Url = docProviders.File_Url,
                    Provider_Type = docProviders.Provider_Type,
                    TRANS_ID = docProviders.TRANS_ID
                };
                return docProvider;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<Domain.DocProviders> GetDocProviderNotMigrated()
        {
            try
            {
                return _oMCSDbContext.DocProviders.Where(doc => doc.File_Is_Migrated == false).ToList().Select(docProviders => new Domain.DocProviders
                {
                    File_Id = docProviders.File_Id,
                    File_Doc_Id = docProviders.File_Doc_Id,
                    Id = docProviders.Id,
                    File_Is_Migrated = docProviders.File_Is_Migrated,
                    File_Status = docProviders.File_Status,
                    File_Url = docProviders.File_Url,
                    Provider_Type = docProviders.Provider_Type,
                    TRANS_ID = docProviders.TRANS_ID
                }).AsQueryable().ToList();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }

        }
        public void Save(Domain.DocProviders docProvider)
        {
            try
            {
                _oMCSDbContext.DocProviders.Add(docProvider);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateDocRepository(Domain.DocProviders docProvider)
        {
            try
            {
                Domain.DocProviders docProviders = GetDocProviderById(docProvider.Id);
                if (docProviders != null)
                {
                    docProviders.File_Doc_Id = docProvider.File_Doc_Id;
                    docProviders.File_Status = docProvider.File_Status; ;
                    docProviders.File_Id = docProvider.File_Id;
                    docProviders.File_Is_Migrated = docProvider.File_Is_Migrated;
                    docProviders.File_Url = docProvider.File_Url;
                    docProviders.Id = docProvider.Id;
                    docProviders.TRANS_ID = docProvider.TRANS_ID;
                    docProviders.Provider_Type = docProvider.Provider_Type;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }

        }
        public Domain.DocProviders GetDocProviderById(int DocProviderId)
        {
            try
            {
                return this.FindBy(t => t.Id == DocProviderId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
    }
}
