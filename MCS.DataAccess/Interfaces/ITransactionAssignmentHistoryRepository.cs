using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITransactionAssignmentHistoryRepository : IRepository<TransactionAssignmentHistory>
    {
        int AddTransactionAssignmentHistory(TransactionAssignmentHistory transactionAssignmentHistory);
        TransactionAssignmentHistory GetTransactionAssignmentHistoryById(int assignmentHistoryId, int? userWeight);

        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistory(int transactionId, string cultureName, int? userWeight);

        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryWithContent(int transactionId, string cultureName, int? userWeight);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryByTransactionId(int transactionId, int? userWeight);
        IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where, int? userWeight);
        IList<TransactionAssignmentHistory> GetUserMobileTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where);
        void UpdateTransactionAssignmentHistoryExplanation(int transId, int ExplanationId, int? userWeight);

        TransactionAssignmentHistory GetLastTransactionAssignmentHistory(int transactionId, int? userWeight, int userId);
      
        void HideTransactionHistory(int assignmentId);
        void HideTransaction(int transactionId);
        void HideTransactionHistories(string assignmentIds);
        void HideTransactions(string transactionIds);
    }
}
