using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using MCS.Business;
using MCS.Business.ProviderModel;
using MCS.Common;
using MCS.DocRepository.DataDef;
using MCS.Framework;

namespace MCS.DocRepository

{
    /// <summary>
    ///	DocRepositoryProvider: The DocRepositoryProvider base class is our contract,
    ///	 Use to implement the desired behaviors for the DocRepository API. 
    /// </summary>
    public abstract class DocRepositoryProvider : ProviderBase
    {
        /// <summary>
        /// GetConfig : Get the names of the providers
        /// </summary>
        /// <returns>ProviderConfiguration object which contains data for The Provider </returns>
        public static ProviderConfiguration GetConfig()
        {
            return (ProviderConfiguration)ConfigurationManager.GetSection("DocRepository");
        }

        /// <summary>
        /// Factory method to instantiate the class on demand.
        /// </summary>
        /// <returns>DocRepositoryProvider Instance </returns>
        public static List<DocRepositoryProvider> Instance()
        {
            List<DocRepositoryProvider> docRepositoryProviders = new List<DocRepositoryProvider>();
            // Get the names of the providers
            ProviderConfiguration config = GetConfig();
            // Read the configuration specific information
            // for this provider
            List<Provider> docRepositoryProviderList = new List<Provider>();
            foreach (var key in config.Providers.Keys)
            {
                var provider = (Provider)config.Providers[key.ToString()];

                docRepositoryProviderList.Add(provider);
            }

            // Load the configuration settings
            object[] paramArray = new object[1];
            foreach (var docRepositoryProvider in docRepositoryProviderList)
            {
                NameValueCollection colDocRepositoryProvider = docRepositoryProvider.Attributes;
                paramArray[0] = colDocRepositoryProvider;
                DocRepositoryProvider docProvider =
                    (DocRepositoryProvider)ReflectionHelper.Create(docRepositoryProvider.AssemblyName,
                        docRepositoryProvider.Type, paramArray);
                docProvider.IsDefault = config.DefaultProvider == docRepositoryProvider.Name;
                docProvider.Name = docRepositoryProvider.Name;
                // Use the cache because the reflection used later is expensive
                docRepositoryProviders.Add(docProvider);
            }

            return docRepositoryProviders;
        }

        public string SaveAndReturnWithDigitalSign(DocData ObjDocData, DocumentLocation ObjDocLocation, bool IsDigitallySigned)
        {
            IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();
            documentBL.UpdateMainDocumentContentWithDigitalSign(Convert.ToInt32(ObjDocData.DocID), ObjDocData.Data, IsDigitallySigned, ObjDocData.MimeContent);
            return "true";
        }

        public bool IsDefault { get; set; }

        public abstract void Save(DocData ObjDocData, DocumentLocation ObjDocLocation);

        public abstract DocData Load(string DocID, DocumentLocation ObjDocLocation);

        public abstract void Delete(string DocID, DocumentLocation ObjDocLocation);

        public abstract string Copy(string DocID, DocumentLocation ObjDocSourceLocation, DocumentLocation ObjDocDestinationLocation);
    }
}