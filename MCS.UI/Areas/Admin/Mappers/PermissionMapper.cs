using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Permission;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class PermissionMapper
    {
        public static List<PermissionDTO> Map(IList<PermissionVM> permissionVMs)
        {
            if (permissionVMs == null || !permissionVMs.Any())
            { return null; }
            List<PermissionDTO> permissionDTOs = permissionVMs
                .Select(b => new PermissionDTO
                {
                    Code = b.Code,
                    groupId = b.groupId,
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    IsUserDefined = b.IsUserDefined,
                    Names = LookupLocalizationMapper.Map(b.Names),
                    Text = b.Text
                     
                }).ToList(); 
            return permissionDTOs;
        }
        public static List<PermissionVM> Map(IList<PermissionDTO> permissionDTOs)
        {
            if (permissionDTOs == null || !permissionDTOs.Any())
            { return null; }
            List<PermissionVM> permissionVMs = permissionDTOs
                .Select(b => new PermissionVM
                { 
                    Code = b.Code,
                    groupId = b.groupId,
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    IsUserDefined = b.IsUserDefined,
                    Names = LookupLocalizationMapper.Map(b.Names),
                    Text = b.Text

                }).ToList();
            return permissionVMs;
        }
        public static PermissionVM Map(PermissionDTO permissionDTO)
        {
            if (permissionDTO != null)
            {
                PermissionVM permissionVM = new PermissionVM()
                {
                    Code = permissionDTO.Code,
                    groupId = permissionDTO.groupId,
                    Id = permissionDTO.Id,
                    IsSelected = permissionDTO.IsSelected,
                    IsUserDefined = permissionDTO.IsUserDefined,
                    Names = LookupLocalizationMapper.Map(permissionDTO.Names),
                    Text = permissionDTO.Text
                };
                return permissionVM;
            }
            return null;
        }
        public static PermissionDTO Map(PermissionVM permissionVM)
        {
            if (permissionVM != null)
            {
                PermissionDTO permissionDTO = new PermissionDTO()
                {
                    Code = permissionVM.Code,
                    groupId = permissionVM.groupId,
                    Id = permissionVM.Id,
                    IsSelected = permissionVM.IsSelected,
                    IsUserDefined = permissionVM.IsUserDefined,
                    Names = LookupLocalizationMapper.Map(permissionVM.Names),
                    Text = permissionVM.Text
                };
                return permissionDTO;
            }
            return null;
        }
        public static List<PermissionEditVM> Map(IList<PermissionEditDTO> permissionEditDTOs)
        {
            if (permissionEditDTOs == null || !permissionEditDTOs.Any())
            { return null; }
            List<PermissionEditVM> permissionEditVMs = permissionEditDTOs
                .Select(b => new PermissionEditVM
                {
                    Names = LookupLocalizationMapper.Map(b.Names),
                     Id = b.Id,
                }).ToList();
            return permissionEditVMs;
        }
        public static List<PermissionEditDTO> Map(IList<PermissionEditVM> permissionEditVMs)
        {
            if (permissionEditVMs == null || !permissionEditVMs.Any())
            { return null; }
            List<PermissionEditDTO> permissionEditDTOs = permissionEditVMs
                .Select(b => new PermissionEditDTO
                {
                    Names = LookupLocalizationMapper.Map(b.Names),
                    Id = b.Id,
                }).ToList();
            return permissionEditDTOs;
        }
        public static PermissionEditDTO Map(PermissionEditVM permissionEditVM)
        {
            if (permissionEditVM != null)
            {
                PermissionEditDTO permissionEditDTO = new PermissionEditDTO()
                {
                    Names = LookupLocalizationMapper.Map(permissionEditVM.Names),
                    Id = permissionEditVM.Id
                };
                return permissionEditDTO;
            }
            return null;
        }
        public static PermissionEditVM Map(PermissionEditDTO permissionEditDTO)
        {
            if (permissionEditDTO != null)
            {
                PermissionEditVM permissionEditVM = new PermissionEditVM()
                {
                    Names = LookupLocalizationMapper.Map(permissionEditDTO.Names),
                    Id = permissionEditDTO.Id
                };
                return permissionEditVM;
            }
            return null;
        }
        public static List<PermissionGroupVM> Map(IList<PermissionGroupDTO> permissionGroupDTOs)
        {
            if (permissionGroupDTOs == null || !permissionGroupDTOs.Any())
            { return null; }
            List<PermissionGroupVM> permissionGroupVMs = permissionGroupDTOs
                .Select(b => new PermissionGroupVM
                {
                    Text = b.Text,
                    Id = b.Id,
                    IsUserDefined = b.IsUserDefined,
                    Permissions = PermissionMapper.Map(b.Permissions)

                }).ToList();
            return permissionGroupVMs;
        }
        public static List<PermissionGroupDTO> Map(IList<PermissionGroupVM> permissionGroupVMs)
        {
            if (permissionGroupVMs == null || !permissionGroupVMs.Any())
            { return null; }
            List<PermissionGroupDTO> permissionGroupDTOs = permissionGroupVMs
                .Select(b => new PermissionGroupDTO
                {
                    Text = b.Text,
                    Id = b.Id,
                    IsUserDefined = b.IsUserDefined,
                    Permissions = PermissionMapper.Map(b.Permissions)

                }).ToList();
            return permissionGroupDTOs;
        }

        public static PermissionGroupVM MapPermissionGroup(PermissionGroupDTO permissionGroupDTO)
        {
            if (permissionGroupDTO == null)
            { return null; }
            PermissionGroupVM permissionGroupVM = new PermissionGroupVM
            {
                Text = permissionGroupDTO.Text,
                Id = permissionGroupDTO.Id,
                IsUserDefined = permissionGroupDTO.IsUserDefined,
                Permissions = PermissionMapper.Map(permissionGroupDTO.Permissions)
            };
            return permissionGroupVM;
        }
    }
}