using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DocRepository.DataDef;
using MCS.Domain;

namespace MCS.DocRepository
{
    public class DocRepository

    {
        public static void Save(DocData ObjDocData, DocumentLocation ObjDocLocation, bool isFirst = false)
        {
            try
            {
                List<DocRepositoryProvider> providerList = DocRepositoryProvider.Instance();
                DocRepositoryProvider docRepositoryProvider = providerList.FirstOrDefault(x => x.IsDefault);
                List<DocRepositoryProvider> docRepositoryProviderList = providerList.Where(x => !x.IsDefault).ToList();

                if (!isFirst)
                {
                    docRepositoryProvider?.Save(ObjDocData, ObjDocLocation);
                }

                if (docRepositoryProviderList.Count > 0)
                {
                    foreach (DocRepositoryProvider oDocRepositoryProvider in docRepositoryProviderList)
                    {
                        DocProviders oDocProviders = new DocProviders()
                        {
                            File_Doc_Id = Convert.ToInt32(ObjDocData.DocID),
                            File_Is_Migrated = false,
                            File_Status = eFileStatus.Insert,
                            Provider_Type = oDocRepositoryProvider.Name,
                            TRANS_ID = ObjDocData.TransactionId
                        };
                        new Business.ProviderModel.DocProviders().Save(oDocProviders);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static string Save(DocData ObjDocData, DocumentLocation ObjDocLocation, bool IsDigitallySigned, bool isFirst = false)
        {
            try
            {
                List<DocRepositoryProvider> providerList = DocRepositoryProvider.Instance();
                DocRepositoryProvider docRepositoryProvider = providerList.FirstOrDefault(x => x.IsDefault);
                List<DocRepositoryProvider> docRepositoryProviderList = providerList.Where(x => !x.IsDefault).ToList();
                string ShrepointId = string.Empty;
                if (!isFirst)
                {
                    ShrepointId = docRepositoryProvider?.SaveAndReturnWithDigitalSign(ObjDocData, ObjDocLocation, IsDigitallySigned);
                }
                return ShrepointId;
                //if (docRepositoryProviderList.Count > 0)
                //{
                //    foreach (DocRepositoryProvider oDocRepositoryProvider in docRepositoryProviderList)
                //    {
                //        DocProviders oDocProviders = new DocProviders()
                //        {
                //            File_Doc_Id = Convert.ToInt32(ObjDocData.DocID),
                //            File_Is_Migrated = false,
                //            File_Status = eFileStatus.Insert,
                //            Provider_Type = oDocRepositoryProvider.Name,
                //            TRANS_ID = Convert.ToInt32(ObjDocData.DocName.Split('_')[0])
                //        };
                //        new Business.ProviderModel.DocProviders().Save(oDocProviders);
                //    }
                //}
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static DocData Load(string DocID, DocumentLocation ObjDocLocation)
        {
            try
            {
                List<DocRepositoryProvider> providerList = DocRepositoryProvider.Instance();

                DocRepositoryProvider docRepositoryProvider = providerList.FirstOrDefault(x => x.IsDefault);

                return docRepositoryProvider?.Load(DocID, ObjDocLocation);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void Delete(string DocID, DocumentLocation ObjDocLocation)
        {
            try
            {
                List<DocRepositoryProvider> providerList = DocRepositoryProvider.Instance();
                DocRepositoryProvider docRepositoryProvider = providerList.FirstOrDefault(x => x.IsDefault);
                List<DocRepositoryProvider> docRepositoryProviderList = providerList.Where(x => !x.IsDefault).ToList();

                docRepositoryProvider?.Delete(DocID, ObjDocLocation);

                foreach (DocRepositoryProvider provider in docRepositoryProviderList)
                {
                    DocProviders providerDoc = new Business.ProviderModel.DocProviders().GetDocProviders(provider.Name, Convert.ToInt32(DocID), eFileStatus.Insert);

                    DocProviders oDocProviders = new DocProviders()
                    {
                        File_Doc_Id = Convert.ToInt32(DocID),
                        File_Is_Migrated = false,
                        File_Status = eFileStatus.Delete,
                        Provider_Type = provider.Name,
                        TRANS_ID = -1,
                        File_Id = providerDoc.File_Id,
                        File_Url = providerDoc.File_Url
                    };
                    new Business.ProviderModel.DocProviders().Save(oDocProviders);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public static string Copy(string DocID, DocumentLocation ObjDocSourceLocation, DocumentLocation ObjDocDestinationLocation)
        {
            try
            {
                List<DocRepositoryProvider> provider = DocRepositoryProvider.Instance();
                DocRepositoryProvider docRepositoryProvider = provider.FirstOrDefault(x => x.IsDefault);

                return docRepositoryProvider?.Copy(DocID, ObjDocSourceLocation, ObjDocDestinationLocation);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsFomService { get; set; }
    }
}