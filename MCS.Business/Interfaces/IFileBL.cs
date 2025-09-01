using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public interface IFileBL
    {
        IList<TrayDetailsInfo> GetUserTrays(int organizationUnitId, string cultureName);
        void MoveTransaction(int transactionId, int organizationUnitId, int trayId, TrayActionType trayActionType, int? assigmentId, string remarks, int userId = 0, string cultureName = "",
            params object[] extraParams);
        IList<TransactionTrayInfo> GetAllUserTransactionsByTray(TrayType traysType, int organizationUnitId, TransactionDateType transactionDate, SearchCriteriaCustom searchCriteria, out int rowsCount);
        TrayDetailsInfo GetTrayDetailsInfo(TrayType trayType, int organizationUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount);
        TrayDetailsInfo GetWithdrawalData(int? transId, int? orgunitId, int? transactionTypeId, int? year, SearchCriteriaCustom searchCriteria, out int rowsCount);
        TransactionAssignment GetTransactionAssignmentLight(int orgUnitId, int transactionId);
        TrayDetailsInfo GetSelectedTransactions(List<int> transactionsIds, string CultureName);
        TransactionDetails CreateOutboundExternal(int transactionId, int trayId, Transaction transactionExternal);
        Transaction PrepareOutboundCreation(int transactionId, int organizationUnitId, int trayId, string cultureName);
        void SendLateTransactionReminderToSender(string cultureName);
        List<Transaction> GetTransactionsByExternalPartyId(int externalPartyId, int orgUnitId);
        void SendLateTransactionWithNotifyLetterTypes(string cultureName);
        void SendNearlyLateTransaction(string cultureName);
        Transaction GetNextTransactionId(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria);
        void LinkedMoveTransaction(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assignmentId, string remarks, int userId = 0, string cultureName = "", params object[] extraParams);

    }
}
