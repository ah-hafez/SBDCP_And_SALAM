using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;

namespace MCS.Service.Mappers
{
    public static class UserPermissionMapper
    {
        public static List<UserPermission> Map(IList<int> permissionIds, int groupId)
        {
            if (permissionIds == null || !permissionIds.Any())
            {
                return null;
            }
            if (permissionIds == null)
            {
                return null;
            }

            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
            List<UserPermission> userPermissions = new List<UserPermission>();

            foreach (int permissionId in permissionIds)
            {
                UserPermission userPermission = new UserPermission()
                {
                    Permission = permissionBL.GetPermissionById(permissionId),
                    GroupId = groupId
                };

                userPermissions.Add(userPermission);
            }

            return userPermissions;
        }
    }
}