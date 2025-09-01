using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IUserPreferenceRepository : IRepository<UserPreference>
    {

        void AddUserPreference(UserPreference userPreference);
        void UpdateUserPreference(UserPreference userPreference);
        void AddUserDelegation(UserDelegation userDelegation, int userId);
        void UpdateUserDelegations(int userId, IList<UserDelegation> userDelegations);
        void UpdateUserDelegation(UserDelegation userDelegation);
        void DeleteDelegation(int id);
        UserPreference GetUserPreferenceByUserId(int userId, string cultureName, int? orgUnitId = null);
        NotificationSubscriptions GetUserNotificationSubscriptions(int userId, string cultureName);
        UserPreference GetUserPreferenceForLogin(int userId, string cultureName);
        UserPreference GetUserPreferenceByUserId(int userId);
        UserPreference GetUserPreference(int userPreferenceId);
        UserDelegation GetUserDelegationById(int id, string cultureName);
        List<UserDelegation> GetUserDelegationsByUserId(int? userId, string cultureName, int? orgUnitId, SearchCriteria searchCriteria, out int rowsCount);
        List<UserDelegation> GetUserDelegations(int preferenceId, SearchCriteria searchCriteria, out int rowsCount);
        AssignmentPaper GetAssignmentPaperByUserId(int userId, string cultureName);
        void AddAssignmentPaper(AssignmentPaper assignmentPaper, int userId);
        void UpdateAssignmentPaper(AssignmentPaper assignmentPaper, int userId);
        bool VerifySignaturePassword(string SignaturePasswordTxt, int userId);
        int AddDistributionList(DistributionList distributionList);
        int SaveDistributionListDetails(List<DistributionListDetails> distributionListDetails, int DistributionListId);
        int UpdateDistributionList(DistributionList distributionList);
        int DeleteDistributionList(int distributionListId);
        List<DistributionList> GetDistributionList(int userId, int orgUnitId);
        DistributionList GetDistributionListById(int userId, int orgUnitId, int id);
        List<UserPreference> GetUserPreferenceByUserIds(List<int> userIds);
        void UpdateTransactionPath(TransactionPath transactionPath);
        List<TransactionPath> GetTransactionPath(int? userId, int? orgUnitId, int pageIndex, int pageSize, string cultureName, out int rowsCount);
        List<TransactionPath> GetAllPaths(int pageIndex, int pageSize, string cultureName, out int rowsCount);
        List<TransactionPath> GetPathsName(int OrgUnitId);
        List<TransactionPath> GetTransactionPathForTransaction(int? userId, int? orgUnitId, string cultureName);
        TransactionPath GetTransactionPathById(int pathId, string cultureName);
        int DeleteTransactionPath(int pathId);
        void UpdateTransactionPathDetailsSort(int pathId, int sort, string order);
        void UpdateUserPreferenceFollowup(int userPreferenceId, int orgUnitId, int? followupOrgUnitId, int? followupUserId);
        #region MobileAPI
        void AddUserSignature(UserPreference userPreference, int userId, string cultureName);
        UserPreference GetUserSignature(int userId, string cultureName);
        bool GenerateVerificationCode(int userId, string code);
        void UpdateUserPreference(int userId, string code);
        void UpdateSignaturePassword(int userId, string signaturePassword, PasswordType passwordType);
        void UpdateSMSNotificationsConfirm(int userId, bool Confirm);
        void UpdatefollowUpUser(int userId, bool Confirm);
        #endregion
        List<Theme> GetTheme();
        string GetThemesById(int id);
        bool GetSMSNotificationsConfirmByUserId(int id);
        bool GetfollowUpUserId(int id);
        List<AssignmentPaperGroup> GetAssignmentPaperGroupsByUserId(int userId);
        void SaveAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup);
        AssignmentPaperGroup GetAssignmentPaperGroupById(int assignmentPaperGroupId);
        void UpdateAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup);
        List<UserDelegation> GetLoggedInUserDelegations(int UserId, string cultureName);

        List<UserDelegation> GetUserDelegationsById(int UserId, string cultureName);

        int AddAllowedAssignment(AllowedAssignment allowedAssignment);

        List<AllowedAssignment> GetAllowedAssignment(int userId, string cultureName);

        bool RemoveAllowedAssignment(int Id);

        AllowedAssignment GetAllowedUserAssignment(int ToUserId, int FromUserId);
        void DeleteAssignmentPaperBeneficiary(int assignmentPaperGroupId);
        void DeleteAssignmentPaperGroup(int assignmentPaperGroupId);
        byte[] GetUserSignByType(int userId, int signType);
        void UpdateUserMobile(int userId, int orgunitId, bool active);
        void ChangeGroupOrder(int id, bool isMoveUp);
        List<AssignmentPaperBeneficiary> GetBeneficiaryByAssignmentPaperGroupId(int groupId);
        void UpdateGroupAssignmentPaper(List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries, int groupId);
        void UpdateGroupAssignmentPaper(List<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries);

        List<AssignmentPaperBeneficiary> GetBeneficiaryByAssignmentPapers();
    }
}
