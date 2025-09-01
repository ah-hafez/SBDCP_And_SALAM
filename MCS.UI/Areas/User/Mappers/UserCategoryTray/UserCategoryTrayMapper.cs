using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.Tray;
using MCS.UI.Areas.User.Models.UserCategories;

namespace MCS.UI.Areas.user.Mappers
{
    public static class UserCategoryTrayMapper
    {
        public static List<UserCategoryTrayDTO> Map(IList<UserCategoryTrayVM> userCategoryTrayVMs)
        {
            if (userCategoryTrayVMs == null || !userCategoryTrayVMs.Any())
            {
                return new List<UserCategoryTrayDTO>();
            }
            List<UserCategoryTrayDTO> userCategoryTrayDTOs = userCategoryTrayVMs
                .Select(b => new UserCategoryTrayDTO
                {
                    Categories = LocalizationMapper.Map(b.Categories),
                    CategoryText = b.CategoryText,
                    Id = b.Id,
                    Trays = TrayMapper.Map(b.Trays)
                }).ToList();
            return userCategoryTrayDTOs;
        }
        public static List<UserCategoryTrayVM> Map(IList<UserCategoryTrayDTO> userCategoryTrayDTOs)
        {
            if (userCategoryTrayDTOs == null || !userCategoryTrayDTOs.Any())
            {
                return new List<UserCategoryTrayVM>();
            }
            List<UserCategoryTrayVM> userCategoryTrayVMs = userCategoryTrayDTOs
                .Select(b => new UserCategoryTrayVM
                {
                    Categories = LocalizationMapper.Map(b.Categories),
                    CategoryText = b.CategoryText,
                    Id = b.Id,
                    Trays = TrayMapper.Map(b.Trays)
                }).ToList();
            return userCategoryTrayVMs;
        }
        public static List<EditUserCategoryTrayDTO> Map(IList<EditUserCategoryTrayVM> editUserCategoryTrayVMs)
        {
            if (editUserCategoryTrayVMs == null || !editUserCategoryTrayVMs.Any())
            {
                return new List<EditUserCategoryTrayDTO>();
            }
            List<EditUserCategoryTrayDTO> editUserCategoryTrayDTOs = editUserCategoryTrayVMs
                .Select(b => new EditUserCategoryTrayDTO
                {
                    TraysIds = b.TraysIds,
                    UserCategoryId = b.UserCategoryId

                }).ToList();
            return editUserCategoryTrayDTOs;
        }
        public static List<EditUserCategoryTrayVM> Map(IList<EditUserCategoryTrayDTO> editUserCategoryTrayDTOs)
        {
            if (editUserCategoryTrayDTOs == null || !editUserCategoryTrayDTOs.Any())
            {
                return new List<EditUserCategoryTrayVM>();
            }
            List<EditUserCategoryTrayVM> editUserCategoryTrayVMs = editUserCategoryTrayDTOs
                .Select(b => new EditUserCategoryTrayVM
                {
                    TraysIds = b.TraysIds,
                    UserCategoryId = b.UserCategoryId
                }).ToList();
            return editUserCategoryTrayVMs;
        }
    }
}