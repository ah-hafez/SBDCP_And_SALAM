using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public interface IUserManagementBL
    {
        //ToDo:enable it when implement asp .net identity
        int AddUser(UserProfile userProfile, string url, string culture);
        int GetAllUsersCount();
        void UpdateUser(UserProfile userProfile, string cultureName);
        void DeleteUsers(IList<int> ids, out IList<int> usersCannotBeDeleted, string cultureName);
        UserProfile GetUserById(int userProfileId);
        UserProfile GetUserByIdentity(string userProfileIdentity, string cultureName);
        int GetUserIdByIdentity(string userProfileIdentity);
        UserProfile GetUserByEmail(string sUserEmail, string cultureName);
        UserProfile GetUserByUserNationalId(string sUserNationalId, string cultureName);
        UserProfile GetUserByIdentity(string userProfileIdentity);
        UserProfile GetUserByUserName(string userName);
        UserProfile GetUserChatByIdentity(string userProfileIdentity);
        IList<UserPermission> GetUserPermissions(int userId, string cultureName);
        IList<UserProfile> GetUsersProfiles(SearchCriteria searchCriteria, out int rowsCount);
        IList<UserProfile> GetPendingRegestrationUsersProfiles(SearchCriteria searchCriteria, out int rowsCount);
        IList<UserProfile> GetUsersByOrgUnitId(int orgUnitId, SearchCriteria searchCriteria, out int rowsCount);
        IList<UserProfile> GetUsersProfiles(Expression<Func<UserProfile, bool>> @where);
        IList<UserProfile> GetUsersProfiles(string cultureName, string searchQuery = null, int? entityId = null);
        IList<Tray> GetUserTrays(int userId);
        IList<UserProfile> GetUsersByPermissionId(int permissionId, string cultureName);
        UserProfile ActivateDeactivateUser(int UserId, string CultureName);
        UserProfile ApproveRequestedUser(int UserId, string CultureName);
        bool RejectRequestedUser(int UserId);
        UserProfile ActivateDeleteUser(int UserId, string CultureName);
        IList<UserProfile> GetUsersByTrayId(int trayId, string cultureName);
        IList<UserProfile> GetUsersByOrgUnitId(int OrgUnitId, string cultureName);
        IList<UserProfile> SearchUsersByOrgUnitId(int? OrgUnitId, string cultureName, string term);
        IList<UserProfile> GetChildEntityUsersByOrgUnitId(int OrgUnitId, string cultureName);
        int AddAssignmentGroup(AssignmentGroup assignmentGroup, string cultureName);
        IList<AssignmentGroup> GetUserAssignmentGroups(int userId, string cultureName);
        AssignmentGroup GetAssignmentGroupById(int groupId);
        AssignmentGroup GetAssignmentGroupById(int groupId, string cultureName);
        string GetUserName(int userId, string cultureName);
        bool ChangePassword(string oldPassword, string newPassword);
        bool CheckIfNotUsedUser(int id);
        void ResetPasswordStepOne(string username, string email, string cultureName, string resetPasswordUrl);
        //ToDo: Check DTO
        void ResetPasswordStepTwo(ResetPasswordDTO resetPasswordDTO);
        void SendUserCreationNotification(UserProfile user, string culture, string url);
        void ActivateUser(UserProfile userProfile, string cultureName);
        void AddUserPreference(UserPreference userPreference, int? orgUnitId = null);
        void UpdateUserPreference(UserPreference userPreference, int? orgUnitId = null);
        bool GenerateVerificationCode(int userId, string cultureName);
        void UpdateUserDelegation(UserDelegation userDelegation, string cultureName);
        void UpdateUserDelegations(int userId, IList<UserDelegation> userDelegations, string cultureName);
        void UpdateUserDelegationStatus(int delegateId, int statusId, string rejectionReason, string cultureName);
        void DeleteDelegations(IList<int> ids);
        UserPreferenceInfo GetUserPreferenceByUserId(int userId, string cultureName, int? orgUnitId = null);
        NotificationSubscriptions GetUserNotificationSubscriptions(int userId, string cultureName = "ar");
        UserPreferenceInfo GetUserPreferenceInfoByUserId(int userId, string cultureName);
        UserPreferenceInfo GetUserPreferenceForLogin(int userId, string cultureName);
        List<UserPreferenceInfo> GetUserPreferenceByUserIds(List<int> userIds);
        UserDelegation GetUserDelegationById(int id, string cultureName);
        List<UserDelegation> GetUserDelegations(int preferenceId, SearchCriteria searchCriteria, out int rowsCount);
        List<UserDelegation> GetUserDelegationsByUserId(int? userId, string cultureName, int? orgUnitId, SearchCriteria searchCriteria, out int rowsCount);
        UserPreference GetUserPreferenceByUserId(int userId);
        int AddUserCategory(UserCategory userCategory);
        void UpdateUserCategory(UserCategory userCategory);
        void DeleteUserCategories(IList<int> ids, out IList<int> userCategoriesCannotBeDeleted);
        void UpdateUsersCategoriesTrays(IList<UserCategoryTray> usersCategoriesTrays);
        UserCategory GetUserCategoryById(int userCategoryId);
        UserCategory GetUserCategoryByUserId(int userId);
        IList<Tray> GetUserCategoryTrays(int userCategoryId);
        IList<Tray> GetUserCategoryTrays(int userCategoryId, string cultureName);
        IList<UserCategory> GetUserCategories(string cultureName);
        IList<UserCategory> GetUserCategories(SearchCriteria searchCriteria, out int rowsCount);
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
        void UpdateTransactionPath(TransactionPath transactionPath, string cultureName);
        List<TransactionPath> GetAllPaths(int pageIndex, int pageSize, string cultureName, out int rowsCount);
        List<TransactionPath> GetTransactionPath(int? userId, int? orgUnitId, int pageIndex, int pageSize, string cultureName, out int rowsCount);
        List<TransactionPath> GetPathsName(int OrgUnitId);
        List<TransactionPath> GetTransactionPathForTransaction(int? userId, int? orgUnitId, string cultureName);
        TransactionPath GetTransactionPathById(int pathId, string cultureName);
        int DeleteTransactionPath(int pathId);
        void UpdateTransactionPathDetailsSort(int pathId, int sort, string order);
        void UpdateUserProfile(int userId, string email);
        void UpdateUserPreference(int userId, string code);
        void UpdateSignaturePassword(string signaturePassword, PasswordType passwordType);
        IList<UserProfile> GetOrgUnitUsers(SearchCriteria searchCriteria, int orgUnitId, string cultureName, out int ItemsCount, bool noExternal = false);
        bool CheckUserNameExists(string userName, string identity, out int? userId);
        string GetThemeByIdForLogin(int ThemeId);
        List<UserGroup> GetUsersWithGroups(string language);
        List<AssignmentPaperGroup> GetAssignmentPaperGroupsByUserId(int userId);
        void SaveAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup);
        AssignmentPaperGroup GetAssignmentPaperGroupById(int assignmentPaperGroupId);
        void UpdateAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup);

        List<UserDelegation> GetLoggedInUserDelegations(int UserId, string cultureName);

        List<UserDelegation> GetUserDelegationsById(int UserId, string cultureName);

        int AddAllowedAssignment(AllowedAssignment allowedAssignment);

        List<AllowedAssignment> GetAllowedAssignment(int UserId, string cultureName);
        bool RemoveAllowedAssignment(int Id);


        AllowedAssignment GetAllowedUserAssignment(int ToUserId, int FromUserId);

        int DeleteAssignmentPaperGroupById(int assignmentPaperGroupId);
        void RemoveSignaturePassword(int userId);
        byte[] GetUserSignByType(int userId, int signType);
        void UpdateUserInternalNumber(int userId, string phoneNumber, string internalNumber);
        void ChangeGroupOrder(int id, bool isMoveUp);
        List<AssignmentPaperBeneficiary> GetBeneficiaryByAssignmentPaperGroupId(int groupId);
        void UpdateAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiary> assignmentPaper, int groupId);
        void AddUserGroup(int userid, int groupId);
        void RemoveUserGroup(int userid, int groupId);

        void UpdateAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiary> assignmentPaper);
        List<UserGroup> GetUsersWithGroups(string language, string GroupId);
        void UserLoginAction(int userID);
        void UserLogoutAction(string userID);
        List<UserProfile> GetUsers(string language);
        void IAMUpdateUser(UserProfile userProfile, string cultureName);


    }
}
