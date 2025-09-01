using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IOrgUnitBL
    {
        void BuildOrgUnitStructure(IList<OrgUnit> organizationUnit, string settings, out IList<int> orgUnitUsedInTransactions);
        OrgUnit GetOrgUnitById(int organizationUnitId, string cultureName);
        OrgUnit GetOrgUnitById(int organizationUnitId);
        OrgUnit GetOrgUnitByExternalId(int externalId);
        IList<OrgUnit> GetOrgUnitLinks(int organizationUnitId, string cultureName);
        List<int> GetAllOrgUnitsId(string cultureName);
        IList<OrgUnit> GetOrgUnitStructure();
        IList<OrgUnit> GetOrgUnits(string cultureName, int? organizationUnitId = null);
        IList<UserProfile> GetUsersByParentId(int OrgUnitId, string cultureName);
        IList<OrgUnit> GetOrgUnits(int? parentId, string cultureName, int LoggedInOrgUnitId, int? UserId = null, OrgUnitTreeMode? orgUnitTreeMode = OrgUnitTreeMode.User);
        IList<OrgUnit> GetOrgUnitsAutoComplete(string searchQuery, string cultureName, int resultSize, int orgUnitId);
        OrgUnit GetOrgUnit(int orgUnitId, string cultureName);
        OrgUnit GetParentOrgUnit(int orgUnitId, string cultureName);
        OrgUnit GetInternalPartyInfoByNumber(string partyNumber, string cultureName);
        List<OrgUnit> GetOrgUnits(List<int> orgUnitIds, string cultureName);
        IList<int> GetOrgUnitsTransactions(IList<int> organizationUnitIds);
        bool CheckOrgUnitUsedInTransaction(int orgUnitId, List<int> transactionCategoryIds);
        OrgUnit ManageOrgUnitAssignmentPaper(OrgUnit orgUnit, AssignmentPaper assignmentPaper);
        string GetOrgUnitName(Expression<Func<OrgUnit, bool>> @where, string cultureName);
        bool CheckOrgUnitHasAssignmentPaper(int organizationUnitId);
        IList<Domain.Action> GetOrgUnitActions(int organizationUnitId, string cultureName);
        IList<Domain.Action> GetOrgUnitActions(int organizationUnitId);
        IList<AssignmentPaperBeneficiary> GetOrgUnitBeneficiaries(int organizationUnitId, string cultureName);
        bool CheckOrgUnitIsAllowedToCreateGroup(int organizationUnitId);
        AssignmentPaper GetAssignmentPaperByOrgUnitId(int organizationUnitId, string cultureName);
        void UpdateAssignmentPaper(AssignmentPaper assignmentPaper);
        BarcodeDesign GetBarcodeDesignByOrgUnitId(int organizationUnitId, int typeId);
        IList<Transaction> GetYearTransactionsCount(int year, int orgUnit, bool isGeneralCounter);
        DateTime GetFirstTransactionDate();
        IList<UserProfile> GetOrgUnitsManagers(string cultureName);
        UserProfile GetOrgUnitManager(int orgUnidId, string cultureName);
        IList<OrgUnit> GetOrgUnitsLight(string cultureName);
        IList<OrgUnit> GetOrgUnitsNew(string cultureName);

        IList<OrgUnit> GetOrgUnitsWithCounter(string cultureName);
        IList<OrgUnit> GetOrgUnitsWithUser(string cultureName);
        IList<OrgUnit> GetOrgUnitsWithLinks(string cultureName);
        void UpdateOrgUnitWithUsers(OrgUnit orgUnit, string cultureName);
        void UpdateOrgUnitWithCounter(OrgUnit orgUnit, string cultureName);
        void UpdateOrgUnitToJoinGeneralCounter(int orgUnitId);
        int UpdateOrgUnitInfo(OrgUnit OrgUnit);
        void UpdateOrgUnitWithLink(OrgUnit orgUnit, string cultureName);
        void UpdateOrgUnitWithBarcodeDesign(OrgUnit orgUnit, string cultureName);
        bool DeleteOrgUnit(int orgUnitKey);
        IList<OrgUnit> GetOrgUnitStructureRoot(int? parentId);

        IList<OrgUnit> GetAllUnitByLineage(string lineage, string cultureName);
        void AdminMoveUser(int userID, int orgunitID, int newOrgunitID, int loggedinUserID);
        void AdminMoveUser(string usersIDs, int orgunitID, int newOrgunitID, int loggedinUserID, bool isExternal = false);
        int MoveEntity(int entityFrom, int entityTo, int loginUser, bool noExternal = false);
        void AdminMoveTransactions(int entityFromId, int entityToId, int userFromId, int userToId, int logInUser);
        void AdminMoveTransactionById(int transId, int toUserId, int toEntityId, int loggedInUser);
        OrgUnit GetOrgUnitsGeneralCounter(string cultureName);
        int MergeDepartments(MergeDepartment mergeDepartment, bool noExternal = false);
        bool CheckOrgUnitIsExternal(int entityId);
        void AdminDeleteUserERP(int userId, int externalOrgunitID, int loggedinUserID);
        bool CheckOrgUnitNumber(string Number, int OrgUnitKey);
        int? getIoDepartment(int orgunitID);
        int? getGeneralIoDepartment();
        int? getFollowUpDepartment(int orgunitID);
        bool ReceiveElcOutBoundWithAcknowled(int orgunitID);
        bool CheckIfOrgunitSendSpecialCopy(int orgunitID);
        IList<UserProfile> GetAllUsers();
        bool ValidateManagerCanAssign(int orgUnitId, int managerUserId, int transactionId, int transactionUserId, bool isManager);
        string GetOrgUnitSymbol(int OrgUnitId);
        void UpdateOrgFromService(IList<OrgunitSap> orgunitSapDtos);
        IList<OrgUnit> IAMGetOrgUnits(string cultureName);
    }
}
