using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Permission;

namespace MCS.UI.Areas.User.Mappers.Permission
{
    public static class PermissionMapper
    {
        public static PermissionEditVM Map(PermissionEditDTO permissionEditDTO)
        {
            if (permissionEditDTO != null)
            {
                PermissionEditVM permissionEditVM = new PermissionEditVM()
                {
                    Id = permissionEditDTO.Id,
                    Names = LookupLocalizationMapper.Map(permissionEditDTO.Names)
                };
                return permissionEditVM;
            }
            return new PermissionEditVM();
        }
        public static PermissionEditDTO Map(PermissionEditVM permissionEditVM)
        {
            if (permissionEditVM != null)
            {
                PermissionEditDTO permissionEditDTO = new PermissionEditDTO()
                {
                    Id = permissionEditVM.Id,
                    Names = LookupLocalizationMapper.Map(permissionEditVM.Names)
                };
                return permissionEditDTO;
            }
            return new PermissionEditDTO();
        }

        public static PermissionGroupVM Map(PermissionGroupDTO permissionGroupDTO)
        {
            if (permissionGroupDTO != null)
            {
                PermissionGroupVM permissionGroupVM = new PermissionGroupVM()
                {
                    Id = permissionGroupDTO.Id,
                    IsUserDefined = permissionGroupDTO.IsUserDefined,
                    Permissions = PermissionMapper.Map(permissionGroupDTO.Permissions),
                    Text = permissionGroupDTO.Text
                };
                return permissionGroupVM;
            }
            return new PermissionGroupVM();

        }

        public static PermissionGroupDTO Map(PermissionGroupVM permissionGroupVM)
        {
            if (permissionGroupVM != null)
            {
                PermissionGroupDTO permissionGroupDTO = new PermissionGroupDTO()
                {
                    Id = permissionGroupVM.Id,
                    IsUserDefined = permissionGroupVM.IsUserDefined,
                    Permissions = PermissionMapper.Map(permissionGroupVM.Permissions),
                    Text = permissionGroupVM.Text
                };
                return permissionGroupDTO;
            }
            return new PermissionGroupDTO();
        }

        public static PermissionVM Map(PermissionDTO permissionDTO)
        {
            if (permissionDTO != null)
            {
                PermissionVM permissionVM = new PermissionVM()
                {
                    Code = permissionDTO.Code,
                    groupId = permissionDTO.groupId,
                    Text = permissionDTO.Text,
                    Id = permissionDTO.Id,
                    IsSelected = permissionDTO.IsSelected,
                    IsUserDefined = permissionDTO.IsUserDefined,
                    Names = LookupLocalizationMapper.Map(permissionDTO.Names)
                };
                return permissionVM;
            }
            return new PermissionVM();
        }

        public static PermissionDTO Map(PermissionVM permissionVM)
        {
            if (permissionVM != null)
            {
                PermissionDTO permissionDTO = new PermissionDTO()
                {
                    Code = permissionVM.Code,
                    groupId = permissionVM.groupId,
                    Text = permissionVM.Text,
                    Id = permissionVM.Id,
                    IsSelected = permissionVM.IsSelected,
                    IsUserDefined = permissionVM.IsUserDefined,
                    Names = LookupLocalizationMapper.Map(permissionVM.Names)
                };
                return permissionDTO;
            }
            return new PermissionDTO();
        }
        public static List<PermissionVM> Map(IList<PermissionDTO> permissionDTOs)
        {
            if (permissionDTOs == null || !permissionDTOs.Any())
            {
                return new List<PermissionVM>();
            }
            List<PermissionVM> permissionVMs = permissionDTOs.Select(permissionVM => new PermissionVM()
            {
                Code = permissionVM.Code,
                groupId = permissionVM.groupId,
                Text = permissionVM.Text,
                Id = permissionVM.Id,
                IsSelected = permissionVM.IsSelected,
                IsUserDefined = permissionVM.IsUserDefined,
                Names = LookupLocalizationMapper.Map(permissionVM.Names)
            }).ToList();

            return permissionVMs;
        }
        public static List<PermissionDTO> Map(IList<PermissionVM> permissionVMs)
        {
            if (permissionVMs == null || !permissionVMs.Any())
            {
                return new List<PermissionDTO>();
            }
            List<PermissionDTO> permissionDTOs = permissionVMs.Select(permissionDTO => new PermissionDTO()
            {
                Code = permissionDTO.Code,
                groupId = permissionDTO.groupId,
                Text = permissionDTO.Text,
                Id = permissionDTO.Id,
                IsSelected = permissionDTO.IsSelected,
                IsUserDefined = permissionDTO.IsUserDefined,
                Names = LookupLocalizationMapper.Map(permissionDTO.Names)
            }).ToList();

            return permissionDTOs;
        }
    }
}