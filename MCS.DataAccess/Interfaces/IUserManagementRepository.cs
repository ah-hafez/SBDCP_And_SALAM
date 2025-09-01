using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IUserManagementRepository : IRepository<UserProfile>
    {
        int AddUser(UserProfile userProfile);
        void UpdateUser(UserProfile userProfile);

        void UpdateUserRoles(UserProfile userProfile);

        UserProfile GetUserById(int userProfileId);
        UserProfile GetUserByIdentity(string userProfileIdentity, string cultureName);
        UserProfile GetUserByEmail(string sUserEmail, string cultureName);
        UserProfile GetUserByUserNationalId(string sUserNationalId, string cultureName);
        UserProfile GetUserByIdentity(string userProfileIdentity);
        UserProfile GetUserChatByIdentity(string userProfileIdentity);
        int GetUserIdByIdentity(string userProfileIdentity);
        IList<UserProfile> GetUsersProfiles(SearchCriteria searchCriteria, out int rowsCount);
        IList<UserProfile> GetPendingRegestrationUsersProfiles(SearchCriteria searchCriteria, out int rowsCount);
        List<UserProfile> GetUsersByOrgUnitId(int orgUnitId, SearchCriteria searchCriteria, out int rowsCount);
        IList<UserProfile> GetUsersProfiles(Expression<Func<UserProfile, bool>> @where);
        IList<UserProfile> GetAllUsers(string cultureName, string searchQuery = null, int? entityId = null);
        IList<UserPermission> GetUserPermissions(int userId, string cultureName);
        IList<Tray> GetUserTrays(int userId);
        IList<UserGroup> GetUserGroup(int userId);
        IList<UserProfile> GetUsersByPermissionId(int permissionId, string cultureName);
        IList<UserProfile> GetUsersByTrayId(int trayId, string cultureName);
        IList<UserProfile> GetUsersByOrgUnitId(int orgUnitId, string cultureName);
        IList<UserProfile> SearchUsersByOrgUnitId(int? orgUnitId, string cultureName, string term);
        UserProfile ActivateDeactivateUser(int UserId, string CultureName);
        UserProfile ApproveRequestedUser(int UserId, string CultureName);
        bool RejectRequestedUser(int UserId);
        UserProfile ActivateDeleteUser(int UserId, string CultureName);
        IList<UserProfile> GetChildEntityUsersByOrgUnitId(int orgUnitId, string cultureName);
        int AddAssignmentGroup(AssignmentGroup assignmentGroup);
        IList<AssignmentGroup> GetUserAssignmentGroups(int userId, string cultureName);
        AssignmentGroup GetAssignmentGroupById(int groupId);
        AssignmentGroup GetAssignmentGroupById(int groupId, string cultureName);
        string GetUserName(int userId, string cultureName);
        UserProfile CheckIfValidUserInfo(string userName, string email);

        UserProfile GetUserByUserName(string userName);

        void ActivateUser(UserProfile userProfile);
        string GetUserIdentityByUserId(int userProfileId);
        int GetAllUsersCount();
        UserCategory GetUserCategoryByUserId(int userId);

        string GetUserLocalNameById(int userId, string cultureName);
        void UpdateUserProfile(int userId, string email);

        void UpdateManger(UserProfile userProfile);

        IList<UserProfile> GetOrgUnitUsers(SearchCriteria searchCriteria, int orgUnitId, string cultureName, out int ItemsCount, bool noExternal = false);
        bool CheckUserNameExists(string userName, string identity, out int? userId);
        List<UserGroup> GetUsersWithGroups();
        void UpdateUserInternalNumber(int userId, string phoneNumber, string internalNumber);
        #region MobileApi
        UserProfile GetUserInfo(string userName, string cultureName);
        bool CheckIfUserHasPermission(int userId, int permissionId);
        IList<Permission> GetUserPrivileges(int userId, string currentUserIdentity, string cultureName);
        void RemoveUserGroup(int userid, int groupId);
        void AddUserGroup(int userid, int groupId);
        #endregion

        List<UserGroup> GetUsersWithGroups(string GroupId);
        void UserLoginAction(int userID);
        void UserLogoutAction(string userID);
        List<UserProfile> GetUsers();
        void IAMUpdateUser(UserProfile userProfile);


    }
}