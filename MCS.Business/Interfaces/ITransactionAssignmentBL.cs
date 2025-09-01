using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public interface ITransactionAssignmentBL
    {
        int AddTransactionAssignment(TransactionAssignment transactionAssignment);
        void UpdateTransactionAssignment(TransactionAssignment transactionAssignment);
        void MoveAllUserTransactions(int UserId);
        void DeleteTransactionAssignments(IList<int> ids);
        void AssignTransaction(IList<Transaction> transactions, IList<TransactionAssignment> transactionAssignments, string culturName = "");
        void AssignTransactionWithdrawal(IList<Transaction> transactions, IList<TransactionAssignment> transactionAssignments, string culturName = "");
        void Assign(int transactionId, int OrgUnitId, string cultureName = "");
        int GetTransactionAssignmentCount(int userId, int trayID, int OrgUnitId, TransactionDateType transactionDateType = TransactionDateType.Any);
        TransactionAssignment GetTransactionAssignmentLight(int userId, int trayId, int OrgUnitId, int transactionId);
        TransactionAssignment GetTransactionAssignmentById(int priorityId);
        TransactionAssignment GetTransactionAssignment(int userId, int transactionID);
        IList<TransactionAssignment> GetTransactionAssignments(int transactionId, string cultureName);
        TransactionAssignment GetLastTransactionAssignments(int transactionId, string cultureName);
        IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> @where);
        IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> @where, string cultureName);
        IList<TransactionAssignmentInfo> GetTransactionAssignmentsInfo(int transactionId, string cultureName);
        void RevertAssignByTransaction(int transactionId, int OrgUnitId, int trayId);
        void RevertReject(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName = "");
        void RevertRejectToCreator(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName);
        void RevertAssignById(int assignmentId, int OrgUnitId);
        IList<TransactionAssignment> GetTransactionAssignments(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount, TrayType trayType, int? transactionDate);
        void SetTransactionAssignmentToViewed(TransactionAssignment transactionAssignment);
        void SetTransactionAssignmentToViewed(int transactionAssignmentId);
        void SetTransactionAssignmentToViewedByTransactionId(int transactionId);
        TransactionPathDetails GetTransactionPathNextStep(int transactionId, string cultureName);
        void SetCopyAsViewed(int transId, int? toUserId, int toOrgUnit);
        TransactionAssignment TransactionDirectReply(int transactionId, string remarks, int userId);
        int GetTransactionAssignmentHistoryCount(int userId, int trayID, int OrgUnitId, TransactionDateType transactionDateType = TransactionDateType.Any);
        int GetMyTransactionTrayCount(int userId, int trayID, int OrgUnitId, TrayProcedureFilter trayProcedureFilter = TrayProcedureFilter.OthersAll, TransactionDateType transactionDateType = TransactionDateType.Any);
        void RejectTransactionMobile(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName, int userId);

    }
}
