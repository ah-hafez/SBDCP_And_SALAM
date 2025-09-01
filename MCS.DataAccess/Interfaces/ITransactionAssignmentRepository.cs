using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;
using MCS.Domain.MobileSearchCriteria;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public interface ITransactionAssignmentRepository : IRepository<TransactionAssignment>
    {
        int AddTransactionAssignment(TransactionAssignment transactionAssignment);
        void UpdateTransactionAssignment(TransactionAssignment transactionAssignment);
        void DeleteTransactionAssignment(int id);
        TransactionAssignment GetTransactionAssignmentById(int transactionAssignmentId);
        TransactionAssignment GetTransactionAssignment(Expression<Func<TransactionAssignment, bool>> @where);
        IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> @where, string cultureName);
        int GetTransactionAssignmentCount(Expression<Func<TransactionAssignment, bool>> where);
        TransactionAssignment GetTransactionAssignmentLight(Expression<Func<TransactionAssignment, bool>> where);
        TransactionAssignment GetTransactionAssignment(UserProfile toUser, Transaction transaction);
        IList<TransactionAssignment> GetAssignments(Expression<Func<TransactionAssignment, bool>> where, SearchCriteriaCustom searchCriteria, out int rowsCount, int? UserWeight, int currentUserId);
        IList<TransactionAssignment> GetTransactionAssignments(int transactionId, string cultureName);
        TransactionAssignment GetLastTransactionAssignments(int transactionId, string cultureName);
        IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> where);
        IList<Transaction> GetUserTransactionsTray(Expression<Func<TransactionAssignment, bool>> where, int? UserWeight, SearchCriteriaCustom searchCriteria,int currentUserId, out int rowsCount);
        IList<Transaction> GetUserTransactionsByTray(Expression<Func<TransactionAssignment, bool>> where, string cultureName, int? transactionCountToRetrieve = null);
        void SetTransactionAssignmentToViewed(TransactionAssignment transactionAssignment);
        void SetTransactionAssignmentToViewed(int transactionAssignmentId);
        IList<Transaction> GetTransactionsByIds(List<int> TransactionsIds, string CultureName, int? UserWeight, int currentUserId);
        void SetTransactionAssignmentToViewedByTransactionId(int transactionId);
        TransactionPathDetails GetTransactionPathNextStep(int transactionId, string cultureName);
        int GetTransactionPathCount(int pathId, bool excludeEntity = false);
        List<Transaction> UserMobileGetUserTransactionsTray(Expression<Func<TransactionAssignment, bool>> where, int? UserWeight, FilterCriteria filterCriteria, string cultureName,int currentUserId, bool isAscending = false);
        TransactionAssignment GetTransactionAssignment(int transactionId, string cultureName, int? UserWeight);
        void MoveAllUserTransactions(int UserId);
        void SetCopyAsViewed(int transId, int? toUserId, int toOrgUnit, string ViewdOnDateH);
        IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> where, SearchCriteriaCustom searchCriteria, out int rowsCount, int? userWeight,int currentUserId);
        int GetTransactionAssignmentHistoryCount(Expression<Func<TransactionAssignment, bool>> where);
        Transaction GetNextTransactionsTray(Expression<Func<TransactionAssignment, bool>> where, SearchCriteriaCustom searchCriteria);
        IList<Transaction> GetTransactionByUsername(Expression<Func<TransactionAssignment, bool>> where, BaseSearchCriteria searchCriteria, out int rowsCount);



    }
}
