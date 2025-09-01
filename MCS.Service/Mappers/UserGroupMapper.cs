using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class UserGroupMapper
    {
        public static List<UserGroup> Map(IList<int> groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return new List<UserGroup>();
            }
            if (groupIds == null)
            {
                return new List<UserGroup>();
            }

            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
            List<UserGroup> userGroups = new List<UserGroup>();

            foreach (int groupId in groupIds)
            {
                UserGroup userGroup = new UserGroup()
                {
                    Group = permissionBL.GetGroupById(groupId)
                };
                userGroups.Add(userGroup);
            }

            return userGroups;
        }

        public static List<UserGroupDTO> Map(List<UserGroup> usersWithGroups, string cultureName)
        {
            List<UserGroupDTO> userGroupDTOList = new List<UserGroupDTO>();
            foreach (UserGroup userWithGroups in usersWithGroups)
            {
                UserGroupDTO userGroupDTO = new UserGroupDTO
                {
                    UserId = userWithGroups.UserId,
                    GroupId = userWithGroups.GroupId,
                    UserName = userWithGroups.User.UserName,
                    GroupName = userWithGroups.Group?.GroupName?.Localizations?.FirstOrDefault(g => g.Culture.ShortName == cultureName)?.Text,
                    AdminUserName = userWithGroups.AdminUserName,
                    Name = userWithGroups.User.LocalizationIdentifier.Localizations.FirstOrDefault(t => t.Culture.ShortName == cultureName).Text,
                    OrgUnitNames = userWithGroups.User.OrgUnits.Select(t => t.LocalizationIdentifier.Localizations.FirstOrDefault(o => o.Culture.ShortName == cultureName).Text).ToList()
                };
                userGroupDTOList.Add(userGroupDTO);
            }
            return userGroupDTOList;
        }



        public static UserGroupDTO Map(UserGroup userGroup)
        {

            UserGroupDTO userGroupDTO = new UserGroupDTO
            {
                UserId = userGroup.UserId,
                GroupId = userGroup.GroupId,
                UserName = userGroup.User == null ? "" : userGroup.User.UserName,
                GroupName = userGroup.Group == null ? "" : userGroup.Group.Name,
            };
            return userGroupDTO;
        }

        public static List<RoleDTO> MapRole(List<Group> groups, string cultureName)
        {
            List<RoleDTO> userGroupDTOList = new List<RoleDTO>();
            foreach (Group group in groups)
            {
                RoleDTO userGroupDTO = new RoleDTO
                {
                    Id = group.Id,
                    LocalName = group.GroupName.Localizations?.FirstOrDefault(g => g.Culture.ShortName == cultureName)?.Text,
                    IsActive = group.IsActive,
                    Users = group?.GroupUsers?.Select(ug => new BasicUserDto
                    {
                        Name = ug?.User?.LocalizationIdentifier?.Localizations?.FirstOrDefault(u => u.Culture.ShortName == cultureName)?.Text,
                        UserId = ug?.User?.Id ?? 0,
                        UserName = ug?.User?.UserName
                    }).ToList(),
                };
                userGroupDTOList.Add(userGroupDTO);
            }
            return userGroupDTOList;
        }

    }
}