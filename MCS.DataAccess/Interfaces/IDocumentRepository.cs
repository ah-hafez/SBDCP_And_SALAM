using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IDocumentRepository : IRepository<DocumentInfo>
    {
        DocumentInfo GetDocumentById(int documentId, int? userWeight);
        void DeleteDocument(int documentId);
        int AddDocument(DocumentInfo documentInfo);
        void UpdateMainDocumentContent(int documentId, int TransactionId, byte[] content, string memType);
        void UpdateDocumentByECMId(string ECMId, int documentId);
        string GetECMIdByDocumentId(int documentId);
        byte[] GetMainDocument(int documentId, int? userWeight);
        void ClearMigratedDocumentBinary(int documentId);
        List<DocumentInfo> GetAllDocuments(int pageSize, int? userWeight);
        void UpdateMainDocumentContentWithDigitalSign(int documentId, byte[] content, bool isDigitallySigned, string mimeContent);
        void UpdateDocumentContentByTransaction(int transactionId, byte[] content);

    }
}
