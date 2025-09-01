using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;
using MCS.Domain.MobileSearchCriteria;

namespace MCS.Business
{
    public interface IUserMobileBL
    {
        UserProfile GetUserInfo(string userName, string cultureName);
        UserProfile GetUserById(int userId);
        UserMobile GetUserMobile(int? userId, string userName, string cultureName);
        void UpdateUserMobile(UserMobile userMobile, string cultureName);
        void UserMobileUpdateTransactionStatus(int transId, int statusId, int userId, int orgUnitId, string reason);
        void UserMobileDeletedTransaction(int transId);
        UserPreference GetUserSignature(int userId, string cultureName);
        void AddUserSignature(UserPreference userPreference, int userId, string cultureName);
        IList<OrgUnit> UserMobileGetOrgHierarchy(int? parentId, string cultureName);
        DocumentInfo GetDocumentById(int documentId, string cultureName);
        List<Permission> GetPermisions(string cultureName, int groupId);
        bool CheckIfUserHasPermission(int userId, string permissionName);
        List<Domain.TransactionType> GetTransactionSources(TransactionCategories transactionCategories, string cultureName);
        List<Action> GetAllActions(string language);
        Transaction CreateTransaction(Transaction transaction, string cultureName);
        Transaction GetTransaction(int transId, string language);
        List<Priority> GetPriorities(TransactionCategories transactionCategories, string language);
        List<LetterType> GetLetterType(TransactionCategories transactionCategories, string language);
        List<AttachmentType> GetAttachementsType(TransactionCategories transactionCategories, string language);
        List<Lookup> GetLookupItems(LookupCategory lookupCategory, string language);
        List<TrayDetailsInfo> GetUserTrays(int userId, int OrgUnitId, string cultureName);
        void AssignItBack(int transactionId, int userId, int OrgUnitId, int trayId, string remarks, string cultureName);
        void AssignItBackVip(int transactionId, int userId, int OrgUnitId, int trayId, string remarks, string cultureName);

        IList<ExternalParty> UserMobileGetExternalParties(int? parentId, string cultureName);
        void SpecializeTransaction(int transactionId, int userId, int OrgUnitId, int trayId, string cultureName);
        TransactionAssignment GetTransactionAssignment(int transId, string cultureName);
        List<TransactionAssignmentHistory> GetTransactionAssignmentHistory(int transId, string cultureName);
        UserAccompleshmentsReportResult GetUserAccompleshmentsReport(int userId, int entityId);
        List<EntitiesAccompleshmentsReportResult> GetEntitiesAccompleshmentsReport(int entityId, int periodCount, int selectedPeriod);
        List<Transaction> UserMobileGetTrayTransactions(int userId, int OrgUnitId, TrayType trayType, TransactionDateType transactionDate, FilterCriteria filterCriteria, string cultureName, bool isAscending = false);
        List<MobileSearchResult> UserMobileSearchTransaction(SearchCriteria searchCriteria, string cultureName);
        List<Explanation> GetTransactionExplanations(int transId, int userId, string cultureName);
        Transaction CreateTransaction(Transaction transaction);
        IList<OrgUnit> UserMobileGetOrgHierarchyAC(string searchQuery, string cultureName);
        IList<ExternalParty> UserMobileGetExternalPartiesAC(string searchQuery, string cultureName);
        IList<Permission> GetUserPrivileges(int userId, string currentUserIdentity, string cultureName);
        AssignmentPaper UserMobileGetAssignmentPaperByUserId(int userId, string cultureName);
        void AssignTransaction(int transactionId, IList<TransactionAssignment> transactionAssignments, string cultureName);
        List<OrgUnit> GetAllEntities(string cultureName, int userId);
        void SetDefaultEntity(int entityId, int userId);
        void AddAssignmentCopies(int transactionId, IList<TransactionCopy> Copies);

    }
}
