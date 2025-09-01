using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.UserCategories;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class UserCategoryMapper
    {
        public static List<UserCategoryDTO> Map(IList<UserCategoryVM> userCategoryVMs)
        {
            if (userCategoryVMs == null || !userCategoryVMs.Any())
            { return null; }
            List<UserCategoryDTO> userCategoryDTOs = userCategoryVMs
                .Select(b => new UserCategoryDTO
                {
                    Categories = LocalizationMapper.Map(b.Categories),
                    CategoryText = b.CategoryText,
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    PermissionText = b.PermissionText

                }).ToList();
            return userCategoryDTOs;
        }
        public static List<UserCategoryVM> Map(IList<UserCategoryDTO> userCategoryDTOs)
        {
            if (userCategoryDTOs == null || !userCategoryDTOs.Any())
            { return null; }
            List<UserCategoryVM> userCategoryVMs = userCategoryDTOs
                .Select(b => new UserCategoryVM
                { 
                    Categories = LocalizationMapper.Map(b.Categories),
                    CategoryText = b.CategoryText,
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    PermissionText = b.PermissionText

                }).ToList();
            return userCategoryVMs;
        }
        public static UserCategoryVM Map(UserCategoryDTO userCategoryDTO)
        {
            if (userCategoryDTO != null)
            {
                UserCategoryVM userCategoryVM = new UserCategoryVM
                {
                    Categories = LocalizationMapper.Map(userCategoryDTO.Categories),
                    CategoryText = userCategoryDTO.CategoryText,
                    Id = userCategoryDTO.Id,
                    IsSelected = userCategoryDTO.IsSelected,
                    PermissionText = userCategoryDTO.PermissionText

                };
                return userCategoryVM;
            }
            return null;
        }
        public static UserCategoryDTO Map(UserCategoryVM userCategoryVM)
        {
            if (userCategoryVM != null)
            {
                UserCategoryDTO userCategoryDTO = new UserCategoryDTO
                {
                    Categories = LocalizationMapper.Map(userCategoryVM.Categories),
                    CategoryText = userCategoryVM.CategoryText,
                    Id = userCategoryVM.Id,
                    IsSelected = userCategoryVM.IsSelected,
                    PermissionText = userCategoryVM.PermissionText

                };
                return userCategoryDTO;
            }
            return null;
        }
        public static List<EditUserCategoryDTO> Map(IList<EditUserCategoryVM> EdituserCategoryVMs)
        {
            if (EdituserCategoryVMs == null || !EdituserCategoryVMs.Any())
            { return null; }
            List<EditUserCategoryDTO> editUserCategoryDTOs = EdituserCategoryVMs
                .Select(b => new EditUserCategoryDTO
                {
                    
                    Id = b.Id,
                    Categories = LocalizationMapper.Map(b.Categories),
                    PermissionId = b.PermissionId

                }).ToList();
            return editUserCategoryDTOs;
        }
        public static EditUserCategoryDTO Map(EditUserCategoryVM EdituserCategoryVMs)
        {
            if (EdituserCategoryVMs != null)
            {
                EditUserCategoryDTO editUserCategoryDTOs = new EditUserCategoryDTO()

                { 
                    Id = EdituserCategoryVMs.Id,
                    Categories = LocalizationMapper.Map(EdituserCategoryVMs.Categories),
                    PermissionId = EdituserCategoryVMs.PermissionId

                };
                return editUserCategoryDTOs;
            }
            return null;
        }
        public static List<EditUserCategoryVM> Map(IList<EditUserCategoryDTO> editUserCategoryDTOs)
        {
            if (editUserCategoryDTOs == null || !editUserCategoryDTOs.Any())
            { return null; }
            List<EditUserCategoryVM> EdituserCategoryVMs = editUserCategoryDTOs
                .Select(b => new EditUserCategoryVM
                { 
                    Id = b.Id,
                    Categories = LocalizationMapper.Map(b.Categories),
                    PermissionId = b.PermissionId

                }).ToList();
            return EdituserCategoryVMs;
        }
        public static EditUserCategoryVM Map(EditUserCategoryDTO editUserCategoryDTO)
        {
            if (editUserCategoryDTO != null)
            {
                EditUserCategoryVM editUserCategoryVM = new EditUserCategoryVM
                {
                    Id = editUserCategoryDTO.Id,
                    Categories = LocalizationMapper.Map(editUserCategoryDTO.Categories),
                    PermissionId = editUserCategoryDTO.PermissionId

                };
                return editUserCategoryVM;
            }
            return null;
        }
        public static AddUserCategoryDTO Map(AddUserCategoryVM AdduserCategoryVM)
        {
            if (AdduserCategoryVM != null)
            {
                return new AddUserCategoryDTO
                { 
                    Categories = LocalizationMapper.Map(AdduserCategoryVM.Categories),
                    PermissionId = AdduserCategoryVM.PermissionId,

                };
            }
            return null;

        }
    }
}