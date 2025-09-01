using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface IEditorBL
    {
        void AddTasks(int transactionId, List<MCS.Domain.Task> tasks, string cultureName);
        IList<Explanation> GetExplanationsByTransactionId(int transactionId, string cultureName);
        IList<Explanation> GetExplanationsCertifByTransactionId(int transactionId, string cultureName);
        TransactionBasicInfo GetTransactionBasicInfo(int transactionId, string cultureName);
        int AddTransactionExplanation(int transactionId, Explanation explanation, string cultureName);
        void AddTransactionLinks(int transactionId, IList<TransactionLink> Links);
        void AddTransactionCopies(int transactionId, IList<TransactionCopy> Copies);
        void AddAssignmentCopies(int transactionId, IList<TransactionCopy> Copies);
        IList<TransactionCopy> GetTransactionCopiesByTransactionId(int transactionId, string cultureName);
        void AssignTransaction(int transactionId, IList<TransactionAssignment> transactionAssignments, string culturName = "");
        void AssignTransactionWithdrawal(int transactionId, IList<TransactionAssignment> transactionAssignments, string culturName = "");
        Transaction GetInboundTransaction(int transactionId, string cultureName);
        TransactionDetails AddTransactionDraft(int transactionId, Transaction transactionDraft);
        IList<TransactionLink> GetTransactionLinks(int transactionId, string cultureName);
        DocumentInfo GetMainDocumentByTransactionId(int transactionId);
        void UpdateTransactionDocument(int transactionId, DocumentInfo documentInfo);
        Explanation GetExplanationById(int explanationId, string cultureName);
        Explanation GetExplanationByDocumentId(int DocumentId, string cultureName);
        Attachment GetAttachmentById(int attachmentId, string cultureName);
        void DeleteExplanation(int explanationId);
        AssignmentPaper GetAssignmentPaperByOrgUnitId(int organizationUnitId, string cultureName);
        void UpdateAssignmentPaper(AssignmentPaper assignmentPaper);
        void UpdateExplanation(Explanation explanation);
        Transaction GetTransaction(int transactionId, int organizationUnitId, string cultureName);
        Transaction GetTransactionLight(int transactionId, int OrgUnitId, string cultureName);
        Transaction GetByTransactionNumber(int transactionNumber);
        void UpdateTransaction(Transaction transaction);
        void UpdateTransactionBasicInfo(int transactionId, TransactionBasicInfo transactionBasicInfo);
        TransactionBasicInfo GetTransactionBasicInfoByNumber(int transactionNumber, int year, int transactionType, string cultureName);
        bool TransactionDirectReply(int transactionId, string remarks, int userId);
        Transaction GetTransaction_VIP(int transactionId, int OrgUnitId, string cultureName);
        void AddEntityDetails(int transactionId, IList<TransactionCopy> Copies);
        DocumentInfo GetOldMainDocumentByTransactionId(int transactionId);
        IList<Explanation> GetExplanationsByTransactionId_New(int transactionId, string cultureName);
        bool CheckTransactionForAssigne(List<int> transactionIds, IList<TransactionAssignment> transactionAssignments);
    }
}
