using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.UserCategories;

namespace MCS.UI.Areas.User.Mappers
{
    public static class UserCategoryMapper
    {
        public static List<UserCategoryDTO> Map(IList<UserCategoryVM> userCategoryVMs)
        {
            if (userCategoryVMs == null || !userCategoryVMs.Any())
            {
                return new List<UserCategoryDTO>();
            }
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
            {
                return new List<UserCategoryVM>();
            }
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
        public static List<EditUserCategoryDTO> Map(IList<EditUserCategoryVM> EdituserCategoryVMs)
        {
            if (EdituserCategoryVMs == null || !EdituserCategoryVMs.Any())
            {
                return new List<EditUserCategoryDTO>();
            }
            List<EditUserCategoryDTO> editUserCategoryDTOs = EdituserCategoryVMs
                .Select(b => new EditUserCategoryDTO
                {
                    Id = b.Id,
                    Categories = LocalizationMapper.Map(b.Categories),
                    PermissionId = b.PermissionId

                }).ToList();
            return editUserCategoryDTOs;
        }
        public static List<EditUserCategoryVM> Map(IList<EditUserCategoryDTO> editUserCategoryDTOs)
        {
            if (editUserCategoryDTOs == null || !editUserCategoryDTOs.Any())
            {
                return new List<EditUserCategoryVM>();
            }
            List<EditUserCategoryVM> EdituserCategoryVMs = editUserCategoryDTOs
                .Select(b => new EditUserCategoryVM
                {
                    Id = b.Id,
                    Categories = LocalizationMapper.Map(b.Categories),
                    PermissionId = b.PermissionId

                }).ToList();
            return EdituserCategoryVMs;
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
            return new AddUserCategoryDTO();
        }
    }
}