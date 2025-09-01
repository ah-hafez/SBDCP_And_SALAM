using System.Collections.Generic;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public interface IDocumentBL
    {
        int AddDocument(DocumentInfo documentInfo);
        DocumentInfo GetDocumentById(int documentId);
        void DeleteDocument(int documentId);
        void UpdateMainDocumentContent(int documentId, int TransactionId, byte[] content,string memType);
        void UpdateDocumentByECMId(string ECMId, int documentId);
        string GetECMIdByDocumentId(int documentId);
        byte[] GetMainDocument(int documentId);
        List<DocumentInfo> GetAllDocuments(int pageSize);
        void ClearMigratedDocumentBinary(int documentId);
        void UpdateMainDocumentContentWithDigitalSign(int v, byte[] data, bool isDigitallySigned, string mimeContent);
        void UpdateDocumentContentByTransaction(int transactionId, byte[] content);


    }
}
