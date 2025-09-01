using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class UsersWithGroupsMapper
    {
        internal static List<UserGroupVM> Map(List<UserGroupDTO> userGroupDTOList)
        {
            List<UserGroupVM> UserGroupList = new List<UserGroupVM>();
            foreach (var userGroupDTO in userGroupDTOList)
            {
                UserGroupVM userGroupVM = new UserGroupVM
                {
                    GroupId = userGroupDTO.GroupId,
                    UserId = userGroupDTO.UserId,
                    UserName = userGroupDTO.UserName,
                    GroupName = userGroupDTO.GroupName,
                    AdminUserName = userGroupDTO.AdminUserName,
                    Name = userGroupDTO.Name,
                    OrgUnitNames = userGroupDTO.OrgUnitNames
                };
                UserGroupList.Add(userGroupVM);
            }

            foreach (var item in UserGroupList)
            {
                foreach (var item1 in item.OrgUnitNames)
                {
                    item.OrgUnitName = item1;
                }

            }
            return UserGroupList;
        }
    }
}