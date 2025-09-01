using Microsoft.Practices.ServiceLocation;
using SolrNet;
using System;
using MCS.Framework.Exceptions;

namespace MCS.Common
{
    public static class SolrIndexer
    {
        #region Attributes

        private static ISolrOperations<TransactionInfo> solrWorker = null;
        private delegate void IndexDelegate(TransactionInfo doc);

        #endregion Attributes

        #region Constructors

        static SolrIndexer()
        {
            new SolrServiceFactory.Instance<TransactionInfo>().Start();

            solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<TransactionInfo>>();          
        }

        #endregion Constructors

        #region Methods

        public static void AddOrUpdate(TransactionInfo doc)
        {
            try
            {
                solrWorker.Add(doc);
                solrWorker.Commit();
                solrWorker.BuildSpellCheckDictionary();
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        public static void AddOrUpdateAsync(TransactionInfo doc)
        {
            try
            {
                IndexDelegate indexDelegate = new IndexDelegate(AddOrUpdate);

                indexDelegate.BeginInvoke(doc, null, null);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        public static void DeleteAll()
        {
            try
            {
                solrWorker.Delete(SolrQuery.All);
                solrWorker.Commit();
                solrWorker.BuildSpellCheckDictionary();
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        public static void DeleteDocument(TransactionInfo doc)
        {
            try
            {
                solrWorker.Delete(doc);
                solrWorker.Commit();
                solrWorker.BuildSpellCheckDictionary();
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }

        #endregion Methods
    }
}
