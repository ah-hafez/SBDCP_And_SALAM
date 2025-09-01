using System;
using System.Collections.Specialized;
using MCS.Framework;
using MCS.Business;
using MCS.DocRepository.DataDef;

namespace MCS.DocRepository.Provider.DB
{
    public class MCSDocRepositoryProvider : DocRepositoryProvider
    {
        public MCSDocRepositoryProvider(NameValueCollection connectionString)
        {

        }

        #region Repository specific behaviors

        public override void Save(DocData ObjDocData, DocumentLocation ObjDocLocation)
        {
            IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();
            ObjDocData.Data = AesWrapper.encrypt(ObjDocData.Data, "Q9s^4E%@", "Q9s^4E%@");
            documentBL.UpdateMainDocumentContent(Convert.ToInt32(ObjDocData.DocID), ObjDocData.TransactionId, ObjDocData.Data, ObjDocData.MimeContent);
        }

        public override DocData Load(string DocID, DocumentLocation ObjDocLocation)
        {
            IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();
            byte[] content = documentBL.GetMainDocument(Convert.ToInt32(DocID));

            DocData ObjDocData = new DocData()
            {
                DocID = DocID,
                Data = content
            };

            try
            {
                ObjDocData.Data = AesWrapper.decrypt(content, "Q9s^4E%@", "Q9s^4E%@");
            }
            catch (Exception)
            {
            }

            return ObjDocData;
        }

        public override void Delete(string DocID, DocumentLocation ObjDocLocation)
        {
            IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();
            documentBL.DeleteDocument(Convert.ToInt32(DocID));
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
