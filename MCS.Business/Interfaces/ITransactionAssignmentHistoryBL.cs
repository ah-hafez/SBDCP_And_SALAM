using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;

namespace MCS.Business
{
    public interface ITransactionAssignmentHistoryBL
    {
        int AddTransactionAssignmentHistory(TransactionAssignment transactionAssignment);
        TransactionAssignmentHistory GetTransactionAssignmentHistoryById(int assignmentHistoryId);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryByTransactionId(int transactionId);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(int transactionId, string cultureName);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryWithContent(int transactionId, string cultureName);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where);
        IList<TransactionAssignmentHistory> GetUserMobileTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where, string cultureName, int userId);
        void UpdateTransactionAssignmentHistory(int transId, int ExplanationId);
        TransactionAssignmentHistory GetLastTransactionAssignmentHistory(int transactionId);
        void HideTransactionAssignment(int assignmentId);
        void HideTransaction(int transactionId);
        void HideTransactionAssignments(string assignmentIds);
        void HideTransactions(string transactionIds);
    }
}
