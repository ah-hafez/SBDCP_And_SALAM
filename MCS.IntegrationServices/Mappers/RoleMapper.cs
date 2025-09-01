using MCS.Business;
using MCS.DTO;
using MCS.Framework;
using MCS.IntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Mappers
{
    public static class RoleMapper
    {
        public static List<RoleModel> Map(IList<PermissionGroupDTO> permissionGroups, bool isSelected = false)
        {
            if (permissionGroups == null || !permissionGroups.Any())
            {
                return null;
            }
            List<RoleModel> permissionsGroupsDTOs = permissionGroups.Select(permissionGroupDTO => new RoleModel()
            {
                Id = permissionGroupDTO.Id,
                Permissions = Map(permissionGroupDTO.Permissions, isSelected),
                Name = permissionGroupDTO.Text
            }).ToList();



            return permissionsGroupsDTOs;
        }

        public static List<PermissionModel> Map(IList<PermissionDTO> permissions, bool isSelected = false)
        {
            if (permissions == null || !permissions.Any())
            {
                return null;
            }
            List<PermissionModel> permissionDTOs = permissions.Select(p => new PermissionModel
            {

                Id = p.Id,
                Name = p.Text


            }).ToList();

            return permissionDTOs;
        }


        //public static List<UserRoleModel> Map(IList<UserGroupDTO> userGroupDTOs, bool isSelected = false)
        //{
        //    if (userGroupDTOs == null || !userGroupDTOs.Any())
        //    {
        //        return null;
        //    }
        //    List<UserRoleModel> userRoleModels = userGroupDTOs.Select(ug => new UserRoleModel
        //    {

        //        UserId = ug.UserId,
        //        RoleId = ug.GroupId,
        //        RoleName = ug.GroupName,
        //        UserName = ug.UserName,


        //    }).ToList();

        //    return userRoleModels;
        //}
        public static List<UserRoleModel> Map(IList<RoleDTO> userGroupDTOs, bool isSelected = false)
        {
            if (userGroupDTOs == null || !userGroupDTOs.Any())
            {
                return null;
            }
            List<UserRoleModel> userRoleModels = userGroupDTOs.Select(ug => new UserRoleModel
            {


                RoleId = ug.Id,
                RoleName = ug.LocalName,
                IsActive = ug.IsActive,
                Users = ug?.Users?.Select(u => new BasicUserResponse
                {
                    Name = u.Name,
                    UserId = u.UserId,
                    Username = u.UserName,

                }).ToList(),


            }).ToList();

            return userRoleModels;
        }

    }
}