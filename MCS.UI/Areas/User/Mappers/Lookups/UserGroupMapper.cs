using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public class UserGroupMapper
    {
        public static List<UserGroupDTO> Map(List<MCS.Domain.UserGroup> usersWithGroups)
        {
            List<UserGroupDTO> userGroupDTOList = new List<UserGroupDTO>();
            foreach (var userWithGroups in usersWithGroups)
            {
                UserGroupDTO userGroupDTO = new UserGroupDTO
                {
                    UserId = userWithGroups.UserId,
                    GroupId = userWithGroups.GroupId,
                    UserName = userWithGroups.User.UserName,
                    GroupName = userWithGroups.Group.Name
                };
                userGroupDTOList.Add(userGroupDTO);
            }
            return userGroupDTOList;
        }

        public static List<UserGroupVM> Map(List<UserGroupDTO> usersWithGroupsDTOList)
        {
            List<UserGroupVM> userGroupDTOList = new List<UserGroupVM>();
            foreach (var userWithGroupsDTO in usersWithGroupsDTOList)
            {
                UserGroupVM userGroupDTO = new UserGroupVM
                {
                    UserId = userWithGroupsDTO.UserId,
                    GroupId = userWithGroupsDTO.GroupId,
                    UserName = userWithGroupsDTO.UserName,
                    GroupName = userWithGroupsDTO.GroupName,
                    Name = userWithGroupsDTO.Name,
                    OrgUnitNames = userWithGroupsDTO.OrgUnitNames
                };
                userGroupDTOList.Add(userGroupDTO);
            }
            return userGroupDTOList;
        }
    }
}