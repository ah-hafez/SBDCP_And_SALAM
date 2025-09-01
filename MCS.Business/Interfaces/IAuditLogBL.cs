using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public interface IAuditLogBL
    {
        int Log(TransactionLog transactionLog);
        IList<TransactionLogInfo> GetTransactionLogInfo(int transactionId, string cultureName);
        IList<AuditLog> GetAuditLog( string cultureName, bool IsForPrint, SearchCriteriaCustom searchCriteria, out int itemsCount);
        IList<TransactionLogDetailInfo> GetAuditLog(int userId, string cultureName);
        TransactionCertificateInfo GetTransactionBasicInfo(int transactionId, string cultureName);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(int transactionId, string cultureName);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryWithContent(int transactionId, string cultureName);
        IList<TransactionCopy> GetTransactionCopiesByTransactionId(int transactionId, string cultureName);
        IList<TransactionExternalCopy> GetTransactionExternalCopiesByTransactionId(int transactionId, string cultureName);
        IList<Explanation> GetExplanationsByTransactionId(int transactionId, string cultureName);
        TransactionAssignment GetTransactionAssignment(int transactionId,  string cultureName);
        IList<TransactionName> GetTransactionNames(int transactionId, string cultureName);
        IList<TransactionLink> GetTransactionLinks(int transactionId, string cultureName);
        IList<TransactionLink> GetTransactionLinksForCertificate(int transactionId, string cultureName);
        IList<Attachment> GetTransactionAttachments(int transactionId, string cultureName);
        TransactionLog GetFirstView(int transactionId, AuditingActionCode auditingActionCode, int? userId, DateTime sendDate, string cultureName);
    }
}
