using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IPermissionBL
    {
        void UpdatePermissions(IList<Permission> permissions);
        IList<Permission> GetPermissions(string cultureName);
        IList<Permission> GetUserPermissionsByGroupId(PermissionGroupName permissionGroupName, string cultureName);
        IList<Permission> GetUserPermissionsByGroupId(PermissionGroupName permissionGroupName);
        IList<Group> GetPermissionsGroups(SearchCriteria searchCriteria, out int rowsCount);
        IList<Group> GetPermissionsGroups(IList<PermissionGroupName> permissionGroupNames, string cultureName);
        IList<Group> GetAllPermissionsGroups(string cultureName, bool includeUserDefinedGroups);
        IList<Group> GetAllUserDefinedGroups(SearchCriteria searchCriteria, out int rowsCount);
        Group GetGroupById(int groupId);
        Permission GetPermissionById(int permissionId);
        Permission GetPermissionByCode(string permissionCode);
        int AddGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteGroups(IList<int> ids);
        void AddPermission(Permission permission);
        void UpdatePermission(Permission permission);
        void DeletePermission(int permissionId);
        Group ActivateDeactivateRole(int RoleId, string CultureName);
        IList<TransactionPathDetails> GetTransactionPathUsersPermissions(int transactionPathId, int permissionId);
        Group GetPermissionsByGroupId(int groupId, string cultureName);
        IList<Permission> GetOutlookUserPermissionsByGroupId(PermissionGroupName permissionGroupName, int userId, string cultureName);
        IList<Permission> GetUserPermissionsByGroupIdMobile(PermissionGroupName permissionGroupName, string cultureName, int userId);
        IList<UserGroup> GetAllUserGroups(SearchCriteria searchCriteria, out int rowsCount);
        IList<Group> GetAllGroups(SearchCriteria searchCriteria, out int rowsCount);
        IList<Group> GetPermissionsGroups_IAM(SearchCriteria searchCriteria, out int rowsCount);

    }
}