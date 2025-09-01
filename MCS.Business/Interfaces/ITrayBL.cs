using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public interface ITrayBL
    {
        void UpdateTray(Tray tray);
        TrayDetailsInfo GetTrayDetailsInfo(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount);
        TransactionAssignment GetTransactionAssignmentLightByOrgUnitIdAndTransactionId(int OrgUnitId, int transactionId);
        IList<TransactionTrayInfo> GetUserTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, out int rowsCount);
        void ApplyTrayAction(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assigmentId);
        void Save(int transactionId, int OrgUnitId, string remarks, string cultureName = "", bool SaveWithComplete = false);
        void Assign(int transactionId, int OrgUnitId, string cultureName = "");
        void RevertAssignTransaction(int transactionId, int OrgUnitId, int trayId);
        void RevertReject(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName = "");
        void RevertRejectToCreator(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName = "");
        void DeleteDraft(int transactionId);
        void SaveRevert(int transactionId, int OrgUnitId);
        void Viewed(int transactionId, int OrgUnitId, int userId, string cultureName = "");
        void DeleteCopy(int transactionId, int OrgUnitId, int userId, string cultureName = "");
        void SetTransactionCopyToUndo(int transactionId, int OrgUnitId, int userId, string cultureName = "");
        void ManagerRevert(int assignmentId, int OrgUnitId);
        void ManagerSave(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assignmentId);
        void ManagerAssign(int transactionId, int assignmentId, IList<TransactionAssignment> transactionAssignments, int OrgUnit, string cultureName = "");
        Transaction PrepareOutboundCreation(int transactionId, int OrgUnitId, string cultureName);
        TransactionDetails CreateOutboundExternal(int transactionId, Transaction transactionExternal);
        TrayDetailsInfo GetPopulariazations(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowCount);
        void FollowUpAddNote(int transactionId, int orgUnitId, int userId, string note);
        TrayDetailsInfo GetWithdrawalData(int? transId, int? orgunitId, int? transactionTypeId, int? year, SearchCriteriaCustom searchCriteria, out int rowsCount);
        Transaction GetNextTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria);
        Transaction GetNextTransaction(int OrgUnitId, SearchCriteriaCustom searchCriteria);
        void LinkedSave(int transactionId, int OrgUnitId, string remarks, int UserId, string cultureName = "", bool SaveWithComplete = false);

    }
}
