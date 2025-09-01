using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;


namespace MCS.DataAccess
{
    public interface IOrgUnitRepository : IRepository<OrgUnit>
    {
        void DeleteOrgUnitLinks(int orgId);
        int AddOrgUnit(OrgUnit orgUnit);
        OrgUnit GetOrgUnitById(int orgUnitId, string cultureName);
        OrgUnit GetOrgUnitById(int orgUnitId);
        void UpdateOrgUnit(OrgUnit orgUnit);
        IList<OrgUnit> GetOrgUnitStructure();
        IList<OrgUnit> GetOrgUnits(string cultureName);
        IList<OrgUnit> GetOrgUnits(int? parentId, string cultureName, int? UserId);
        IList<OrgUnit> GetOrgUnitsAutoComplete(string searchQuery, string cultureName, int resultSize, int orgUnitId, bool isAllModules, bool isParentModule, bool isAllChildsModules);
        IList<UserProfile> GetUsersByOrgUnitId(int orgUnitId, string cultureName);
        OrgUnit GetOrgUnit(int orgUnitId, string cultureName);
        OrgUnit GetParentOrgUnit(int orgUnitId, string cultureName);
        OrgUnit GetInternalPartyInfoByNumber(string partyNumber, string cultureName);
        List<OrgUnit> GetOrgUnits(List<int> orgUnitIds, string cultureName);
        OrgUnitLink GetOrgUnitLink(int orgUnitFromId, int orgUnitToId);
        string GetOrgUnitName(Expression<Func<OrgUnit, bool>> @where, string cultureName);
        bool CheckOrgUnitHasAssignmentPaper(int orgUnitId);
        bool CheckOrgUnitIsAllowedToCreateGroup(int orgUnitId);
        IList<Domain.Action> GetOrgUnitActions(int orgUnitId, string cultureName);
        IList<Domain.Action> GetOrgUnitActions(int orgUnitId);
        IList<AssignmentPaperBeneficiary> GetOrgUnitBeneficiaries(int orgUnitId, string cultureName);
        AssignmentPaper GetAssignmentPaperByOrgUnitId(int orgUnitId, string cultureName);
        void UpdateAssignmentPaper(AssignmentPaper assignmentPaper);
        AssignmentPaper GetAssignmentPaperById(int assignmentPaperId);
        AssignmentPaperAction GetAssignmentPaperActionById(int assignmentPaperActionId);
        AssignmentPaperBeneficiary GetAssignmentPaperBeneficiaryById(int assignmentPaperBeneficiaryId);
        BarcodeDesign GetBarcodeDesignByOrgUnitId(int orgUnitId, int typeId);
        Transaction GetYearTransactionsCount(Expression<Func<Transaction, bool>> @where);
        DateTime GetFirstTransactionDate();
        int GetAllOrgUnitsCount();
        IList<UserProfile> GetOrgUnitsManagers(string cultureName);
        UserProfile GetOrgUnitManager(int orgUnitId, string cultureName);

        IList<OrgUnit> GetOrgUnitsLight(string cultureName);
        IList<OrgUnit> GetOrgUnitsNew(string cultureName);

        List<int> GetAllOrgUnitsId(string cultureName);
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
        bool CheckOrgUnitNumber(string Number, int OrgUnitKey);
        IList<OrgUnit> GetOrgUnitStructureRoot(int? parentId);
        int? getIoDepartment(int orgunitID);
        int? getGeneralIoDepartment();
        int? getFollowUpDepartment(int orgunitID);
        bool ReceiveElcOutBoundWithAcknowled(int orgunitID);
        bool CheckIfOrgunitSendSpecialCopy(int orgunitID);
        bool ValidateManagerCanAssign(int orgUnitId, int managerId, int transactionId, int transactionUserId, bool isManager);

        #region MobileApi
        IList<OrgUnit> UserMobileGetOrgHierarchy(int? parentId, string cultureName);
        IList<OrgUnit> UserMobileGetOrgHierarchyAC(string searchQuery, string cultureName, int resultSize);
        #endregion

        IList<UserProfile> GetAllUsers();
        string GetOrgUnitSymbol(int OrgUnitId);
        void UpdateOrgFromService(IList<OrgunitSap> orgunitSaps);
    }
}
