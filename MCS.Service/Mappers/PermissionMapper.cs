using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class PermissionMapper
    {
        public static PermissionDTO Map(Permission permission, bool isSelected = false)
        {
            if (permission == null)
                return null;

            PermissionDTO permissionDTO = new PermissionDTO()
            {
                Id = permission.Id,
                Code = permission.Code,
                Text = permission?.Name?.Localizations?.Where(p => p.Culture.ShortName == "ar").FirstOrDefault()?.Text ?? permission.LocalName,
                IsSelected = isSelected,
                IsUserDefined = permission.IsUserDefined,

            };

            return permissionDTO;
        }

        public static PermissionEditDTO MapEdit(Permission permission)
        {
            if (permission == null)
                return null;

            return new PermissionEditDTO()
            {
                Id = permission.Id,
                Names = LookupLocalizationMapper.Map(permission.Name.Localizations)
            };
        }

        public static Permission Map(PermissionDTO permissionDTO)
        {
            if (permissionDTO == null)
                return null;

            Permission permission = new Permission()
            {
                PermissionGroups = new List<MCS.Domain.Group>(),
                Name = new Lookup(),
                Code = permissionDTO.Code
            };

            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

            MCS.Domain.Group group = permissionBL.GetGroupById(permissionDTO.groupId);

            permission.Name.Localizations = LookupLocalizationMapper.Map(permissionDTO.Names);

            foreach (LookupLocalization lookupLocalization in permission.Name.Localizations)
            {
                lookupLocalization.Lookup = permission.Name;
            }

            permission.PermissionGroups.Add(group);

            return permission;
        }

        public static Permission Map(PermissionDTO permissionDTO, string cultureName)
        {
            if (permissionDTO == null)
                return null;

            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

            Permission permission = permissionBL.GetPermissionById(permissionDTO.Id);

            if (permission != null && permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault() != null)
            {
                permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text =
                    permissionDTO.Text;
            }

            return permission;
        }

        public static Permission Map(PermissionEditDTO permissionEditDTO)
        {
            if (permissionEditDTO == null)
                return null;

            Permission permission = new Permission()
            {
                Id = permissionEditDTO.Id,
                Name = new Lookup()
            };

            permission.Name.Localizations = LookupLocalizationMapper.Map(permissionEditDTO.Names);

            return permission;
        }

        public static List<Permission> Map(IList<PermissionEditDTO> permissionEditDTOs)
        {
            if (permissionEditDTOs == null || !permissionEditDTOs.Any())
            {
                return null;
            }
            List<Permission> permissions = permissionEditDTOs.Select(permission => new Permission()
            {
                Id = permission.Id,
                Name = new Lookup()
                {
                    Localizations = LookupLocalizationMapper.Map(permission.Names)
                }
            }).ToList();

            return permissions;
        }

        public static List<PermissionDTO> Map(IList<Permission> permissions, bool isSelected = false)
        {
            if (permissions == null || !permissions.Any())
            {
                return null;
            }
            List<PermissionDTO> permissionDTOs = new List<PermissionDTO>();

            foreach (Permission permission in permissions)
            {
                permissionDTOs.Add(PermissionMapper.Map(permission, isSelected));
            }

            return permissionDTOs;
        }

        public static List<Permission> Map(IList<PermissionDTO> permissionDTOs, string cultureName)
        {
            if (permissionDTOs == null || !permissionDTOs.Any())
            {
                return null;
            }
            List<Permission> permissions = new List<Permission>();

            if (permissionDTOs != null)
            {
                foreach (PermissionDTO permissionDTO in permissionDTOs)
                {
                    Permission permission = PermissionMapper.Map(permissionDTO, cultureName);

                    permissions.Add(permission);
                }
            }

            return permissions;
        }

        public static List<PermissionGroupDTO> Map(IList<Group> permissionGroups, bool isSelected = false)
        {
            if (permissionGroups == null || !permissionGroups.Any())
            {
                return null;
            }
            List<PermissionGroupDTO> permissionsGroupsDTOs = permissionGroups.Select(permissionGroupDTO => new PermissionGroupDTO()
            {
                Id = permissionGroupDTO.Id,
                IsUserDefined = permissionGroupDTO.IsUserDefined,
                Permissions = PermissionMapper.Map(permissionGroupDTO.Permissions, isSelected),
                Text = permissionGroupDTO.Name
            }).ToList();



            return permissionsGroupsDTOs;
        }
        public static PermissionGroupDTO MapPermissionGroup(Group permissionGroup, bool isSelected = false)
        {
            if (permissionGroup == null)
            {
                return null;
            }
            PermissionGroupDTO permissionsGroupsDTO = new PermissionGroupDTO()
            {
                Id = permissionGroup.Id,
                IsUserDefined = permissionGroup.IsUserDefined,
                Permissions = PermissionMapper.Map(permissionGroup.Permissions, isSelected),
                Text = permissionGroup.Name
            };

            return permissionsGroupsDTO;
        }
        public static EditGroupDTO Map(Group group)
        {
            if (group == null)
                return null;

            EditGroupDTO groupEditDTO = new EditGroupDTO
            {
                Id = group.Id,
                Name = new LookupDTO { Localizations = LookupLocalizationMapper.Map(group.GroupName.Localizations.ToList()) },
                Permissions = group.Permissions.Select(g => g.Id).ToList()
            };

            return groupEditDTO;
        }

        public static Group Map(AddGroupDTO groupAdd)
        {
            if (groupAdd == null)
                return null;

            Group group = new Group
            {
                GroupName = LookupMapper.Map(groupAdd.Name),
                IsUserDefined = true,
                IsActive = true,
                Permissions = groupAdd.Permissions != null ? PermissionMapper.MapPermissions(groupAdd.Permissions) : new List<Permission>()
            };

            return group;
        }

        public static Group Map(EditGroupDTO groupEdit)
        {
            if (groupEdit == null)
                return null;

            Group group = new Group
            {
                Id = groupEdit.Id,
                GroupName = LookupMapper.Map(groupEdit.Name),
                IsUserDefined = true,
                Permissions = groupEdit.Permissions != null ? PermissionMapper.MapPermissions(groupEdit.Permissions) :
                null
            };

            return group;
        }

        public static List<GroupDTO> MapGroups(IList<Group> permissionGroups, bool isSelected = false)
        {
            if (permissionGroups == null || !permissionGroups.Any())
            {
                return null;
            }
            List<GroupDTO> permissionsGroupsDTOs = permissionGroups.Select(permissionGroupDTO => new GroupDTO()
            {
                Id = permissionGroupDTO.Id,
                LocalName = permissionGroupDTO.Name,
                IsActive = permissionGroupDTO.IsActive,
                Name = LookupMapper.Map(permissionGroupDTO?.GroupName)
            }).ToList();

            return permissionsGroupsDTOs;
        }

        public static GroupDTO MapGroup(Group permissionGroup, bool isSelected = false)
        {
            if (permissionGroup == null)
            {
                return null;
            }
            GroupDTO permissionsGroupDTO = new GroupDTO()
            {
                Id = permissionGroup.Id,
                LocalName = permissionGroup.Name,
                IsActive = permissionGroup.IsActive,
                Name = LookupMapper.Map(permissionGroup?.GroupName)
            };

            return permissionsGroupDTO;
        }

        public static List<Permission> MapPermissions(IList<int> permissionsIds)
        {
            if (permissionsIds == null || !permissionsIds.Any())
            {
                return null;
            }
            List<Permission> permissions = new List<Permission>();
            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

            foreach (int id in permissionsIds)
            {
                permissions.Add(permissionBL.GetPermissionById(id));
            }

            return permissions;
        }

        public static List<PermissionDTO> MapUserPermissions(IList<UserPermission> userPermissions)
        {
            if (userPermissions == null || !userPermissions.Any())
            {
                return null;
            }
            List<PermissionDTO> permissionDTO = new List<PermissionDTO>();

            foreach (UserPermission userPermission in userPermissions)
            {
                permissionDTO.Add(PermissionMapper.Map(userPermission.Permission));
            }

            return permissionDTO;
        }

        public static List<Permission> MapPermissions(IList<PermissionGroupDTO> permissionGroupDTOs, string cultureName)
        {
            if (permissionGroupDTOs == null || !permissionGroupDTOs.Any())
            {
                return null;
            }
            List<Permission> permissions = new List<Permission>();

            if (permissionGroupDTOs != null)
            {
                foreach (PermissionGroupDTO permissionGroupDTO in permissionGroupDTOs)
                {
                    if (permissionGroupDTO.IsUserDefined)
                        continue;

                    IList<Permission> mappedPermissions = PermissionMapper.Map(permissionGroupDTO.Permissions, cultureName);

                    foreach (Permission permission in mappedPermissions)
                    {
                        permissions.Add(permission);
                    }
                }
            }

            return permissions;
        }
    }
}