using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;


namespace MCS.Business
{
    public class PermissionBL : BaseBL, IPermissionBL
    {
        public void DeletePermission(int permissionId)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                Permission permission = permissionRepository.GetPermissionById(permissionId);

                if (permission == null)
                {
                    throw new BusinessException(StatusCode.PermissionNotFound);
                }

                if (!permission.IsUserDefined)
                {
                    throw new BusinessException(StatusCode.DeletePermissionNotAllow);
                }

                int transactionCount = TransactionBL.GetTransactions(t => t.TransactionType.PermissionId == permissionId).Count;

                if (transactionCount > 0)
                {
                    throw new BusinessException(StatusCode.PermissionRelatedToTransactions);
                }

                int expalantionCount = new EditorBL().GetExplanationCount(permissionId);

                if (expalantionCount > 0)
                {
                    throw new BusinessException(StatusCode.PermissionRelatedToExplanation);
                }
                permissionRepository.DeletePermission(permissionId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void AddPermission(Permission permission)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                permission.IsUserDefined = true;

                string code = permission.PermissionGroups.FirstOrDefault().Permissions.FirstOrDefault().Code;

                if (!string.IsNullOrEmpty(code))
                {

                    string prefix = UserClaims.GetClaimPrefix(code);

                    string permissionName = permission.Name.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).FirstOrDefault().Text;

                    permissionName = permissionName.Trim();

                    permission.Code = UserClaims.GenerateClaimCode(prefix, permissionName);

                    permissionRepository.AddPermission(permission);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdatePermission(Permission permission)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                permissionRepository.UpdatePermission(permission);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public Group ActivateDeactivateRole(int RoleId, string CultureName)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.ActivateDeactivateRole(RoleId, CultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdatePermissions(IList<Permission> permissions)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                permissionRepository.UpdatePermissions(permissions);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Permission> GetPermissions(string cultureName)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetPermissions(cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Permission> GetUserPermissionsByGroupId(PermissionGroupName permissionGroupName, string cultureName)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetUserPermissionsByGroupId((int)permissionGroupName, User.Id, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Permission> GetUserPermissionsByGroupId(PermissionGroupName permissionGroupName)
        {
            try
            {
                List<Permission> userPermissions = new List<Permission>();
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                List<Permission> permissions = permissionRepository.GetPermissions(p => p.PermissionGroups.Any(g => g.Id == (int)permissionGroupName)).ToList();

                if (permissions != null)
                {
                    foreach (Permission permission in permissions)
                    {
                        if (User.Claims.Where(c => c.Name == permission.Code).FirstOrDefault() != null)
                            userPermissions.Add(permission);
                    }
                }
                return userPermissions;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Group> GetPermissionsGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetPermissionsGroups(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<Group> GetPermissionsGroups_IAM(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetPermissionsGroups_IAM(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        
        public IList<Group> GetPermissionsGroups(IList<PermissionGroupName> permissionGroupNames, string cultureName)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                IList<Group> permissionsGroups = new List<Group>();

                if (permissionGroupNames != null)
                {
                    foreach (PermissionGroupName permissionGroupName in permissionGroupNames)
                    {
                        permissionsGroups.Add(permissionRepository.GetPermissionsGroupById((int)permissionGroupName, cultureName));
                    }
                }
                return permissionsGroups;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Group> GetAllPermissionsGroups(string cultureName, bool includeUserDefinedGroups)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetAllPermissionsGroups(cultureName, includeUserDefinedGroups);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<Group> GetAllUserDefinedGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetAllUserDefinedGroups(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Group GetGroupById(int groupId)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetPermissionsGroupById(groupId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Permission GetPermissionById(int permissionId)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetPermissionById(permissionId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Permission GetPermissionByCode(string permissionCode)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetPermissionByCode(permissionCode);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public int AddGroup(Group group)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.AddGroup(group);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Group GetPermissionsByGroupId(int groupId, string cultureName)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetPermissionsByGroupId(groupId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateGroup(Group group)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                permissionRepository.UpdateGroup(group);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void DeleteGroups(IList<int> ids)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                foreach (int id in ids)
                {
                    permissionRepository.DeleteGroup(id);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<TransactionPathDetails> GetTransactionPathUsersPermissions(int transactionPathId, int permissionId)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetTransactionPathUsersPermissions(transactionPathId, permissionId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Permission> GetOutlookUserPermissionsByGroupId(PermissionGroupName permissionGroupName, int userId, string cultureName)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetUserPermissionsByGroupId((int)permissionGroupName, userId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<Permission> GetUserPermissionsByGroupIdMobile(PermissionGroupName permissionGroupName, string cultureName, int userId)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetUserPermissionsByGroupId((int)permissionGroupName, userId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserGroup> GetAllUserGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetUsersGroups(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Group> GetAllGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetAllGroups(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
