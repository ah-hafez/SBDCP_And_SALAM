using System;
using System.Collections.Specialized;
using MCS.Framework;
using MCS.Framework.MultiTenants;
using MCS.Business;
using MCS.Common;
using MCS.DocRepository.DataDef;
using MCS.DocRepository.Provider.ECM.ECMService;

namespace MCS.DocRepository.Provider.ECM
{
    public class ECMDocRepositoryProvider : DocRepositoryProvider
    {
        string xIdcProfile = System.Configuration.ConfigurationManager.AppSettings["xIdcProfile"];
        string xCategoryID = System.Configuration.ConfigurationManager.AppSettings["xCategoryID"];
        ECMServiceClient ECMServiceClient = new ECMServiceClient();
        public ECMDocRepositoryProvider(NameValueCollection connectionString)
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {

                xIdcProfile = TenantHelper.GetECMProfileIdFromHeader();
                xCategoryID = TenantHelper.GetECMCategoryIdFromHeader();

            }
        }

        #region Repository specific behaviors

        public override void Save(DocData ObjDocData, DocumentLocation ObjDocLocation)
        {
            try
            {
                if (ObjDocData.ECMID == null)
                {
                    CheckInNewRequestData checkInNewRequestData = new CheckInNewRequestData();

                    checkInNewRequestData.ProfileID = xIdcProfile;
                    checkInNewRequestData.AttachmentName = String.IsNullOrWhiteSpace(ObjDocData.DocName) ? "AttachmentName" : ObjDocData.DocName;
                    checkInNewRequestData.DocumentTitle = String.IsNullOrWhiteSpace(ObjDocData.DocName) ? "DocumentTitle" : ObjDocData.DocName;
                    checkInNewRequestData.CategoryID = xCategoryID;
                    checkInNewRequestData.AttachmentContent = ObjDocData.Data;
                    checkInNewRequestData.DocID = ObjDocData.DocID;
                    checkInNewRequestData.DocumentType = DocumentType.Document;
                    checkInNewRequestData.ExtensionData = null;
                    checkInNewRequestData.MOR_UserID = ObjDocData.User_ID;
                    checkInNewRequestData.MOR_SourceEntity = ObjDocData.EntityId + "";
                    checkInNewRequestData.MOR_DestinationEntity = ObjDocData.EntityId + "";
                    checkInNewRequestData.MOR_TransactionID = ObjDocData.TransactionId + "";
                    checkInNewRequestData.MOR_TransactionDate = ObjDocData.TransactionDate;
                    checkInNewRequestData.MOR_TransactionDateHijri = ObjDocData.TransactionDateHijri;

                    CheckInNewResponseData ECMResponse = ECMServiceClient.Insert(checkInNewRequestData);

                    IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();
                    documentBL.UpdateDocumentByECMId(ECMResponse.DocumentName, Convert.ToInt32(ObjDocData.DocID));
                }
                else
                {
                    CheckInUniversalRequestData checkInUniversalRequestData = new CheckInUniversalRequestData();

                    checkInUniversalRequestData.ProfileID = xIdcProfile;
                    checkInUniversalRequestData.AttachmentName = String.IsNullOrWhiteSpace(ObjDocData.DocName) ? "AttachmentName" : ObjDocData.DocName;
                    checkInUniversalRequestData.DocumentTitle = String.IsNullOrWhiteSpace(ObjDocData.DocName) ? "DocumentTitle" : ObjDocData.DocName;
                    checkInUniversalRequestData.CategoryID = xCategoryID;
                    checkInUniversalRequestData.AttachmentContent = ObjDocData.Data;
                    checkInUniversalRequestData.DocID = ObjDocData.DocID;
                    checkInUniversalRequestData.DocumentType = DocumentType.Document;
                    checkInUniversalRequestData.ExtensionData = null;
                    checkInUniversalRequestData.MOR_UserID = ObjDocData.User_ID;
                    checkInUniversalRequestData.MOR_SourceEntity = ObjDocData.EntityId + "";
                    checkInUniversalRequestData.MOR_DestinationEntity = ObjDocData.EntityId + "";
                    checkInUniversalRequestData.MOR_TransactionID = ObjDocData.TransactionId + "";
                    checkInUniversalRequestData.MOR_TransactionDate = ObjDocData.TransactionDate;
                    checkInUniversalRequestData.MOR_TransactionDateHijri = ObjDocData.TransactionDateHijri;
                    checkInUniversalRequestData.DocumentName = ObjDocData.ECMID;

                    ECMServiceClient.Update(checkInUniversalRequestData);
                }
            }
            catch (Exception ex)
            {
            }

        }

        public override DocData Load(string DocID, DocumentLocation ObjDocLocation)
        {
            IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();


            GetFileRequestData getFileRequestData = new GetFileRequestData()
            {
                DocumentName = documentBL.GetECMIdByDocumentId(Convert.ToInt32(DocID))
            };

            GetFileResponseData getFileResponseData = ECMServiceClient.GetFile(getFileRequestData);

            return new DocData()
            {
                DocID = getFileResponseData.DocID,
                Data = getFileResponseData.AttachmentContent,
                ECMID = getFileRequestData.DocumentName,
                EntityId = Convert.ToInt32(getFileResponseData.MOR_SourceEntity),
                User_ID = getFileResponseData.MOR_UserID,
                Application = "",
                DocName = ""
            };
        }

        public override void Delete(string DocID, DocumentLocation ObjDocLocation)
        {

        }

        public override string Copy(string DocID, DocumentLocation ObjDocSourceLocation, DocumentLocation ObjDocDestinationLocation)
        {
            return DocID;
        }

        #endregion

        #region Provider specific behaviors
        public override void Initialize(string name, NameValueCollection configValue)
        {

        }

        public override string Name { get; set; }

        #endregion
    }
}
