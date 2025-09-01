using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        void UpdatePermissions(IList<Permission> permissions);
        IList<Permission> GetPermissions(string cultureName);
        IList<Permission> GetPermissions(Expression<Func<Permission, bool>> @where, string cultureName);
        IList<Permission> GetPermissions(Expression<Func<Permission, bool>> @where);
        IList<Permission> GetUserPermissionsByGroupId(int groupId, int userId, string cultureName);
        IList<Group> GetPermissionsGroups(SearchCriteria searchCriteria, out int rowsCount);
        Group GetPermissionsGroupById(int permissionGroupId);
        Group GetPermissionsGroupById(int permissionGroupId, string cultureName);
        IList<Group> GetAllPermissionsGroups(string cultureName, bool includeUserDefinedGroups);
        IList<Group> GetAllUserDefinedGroups(SearchCriteria searchCriteria, out int rowsCount);
        Permission GetPermissionById(int permissionId);
        Permission GetPermissionByCode(string permissionCode);
        int AddGroup(Group group);
        void UpdateGroup(Group group);
        void DeleteGroup(int id);
        void AddPermission(Permission permission);
        void UpdatePermission(Permission permission);
        void DeletePermission(int permissionId);
        Group ActivateDeactivateRole(int RoleId, string CultureName);
        IList<TransactionPathDetails> GetTransactionPathUsersPermissions(int transactionPathId, int permissionId);
        Group GetPermissionsByGroupId(int groupId, string cultureName);
        IList<UserGroup> GetUsersGroups(SearchCriteria searchCriteria, out int rowsCount);
        IList<Group> GetAllGroups(SearchCriteria searchCriteria, out int rowsCount);
        IList<Group> GetPermissionsGroups_IAM(SearchCriteria searchCriteria, out int rowsCount);
    }
}
