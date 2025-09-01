using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class UserCategoryMapper
    {
        public static UserCategory Map(AddUserCategoryDTO userCategoryAddDTO)
        {
            if (userCategoryAddDTO == null)
            {
                return null;
            }
            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

            UserCategory userCategory = new UserCategory()
            {


                CategoryName = userCategoryAddDTO.Categories != null ? LocalizationIdentifierMapper.Map(userCategoryAddDTO.Categories) : null,
                Permission = permissionBL.GetPermissionById(userCategoryAddDTO.PermissionId)
            };

            return userCategory;
        }

        public static UserCategory Map(EditUserCategoryDTO userCategoryEditDTO)
        {
            if (userCategoryEditDTO == null)
            {
                return null;
            }
            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

            UserCategory userCategory = new UserCategory()
            {
                Id = userCategoryEditDTO.Id,
                CategoryName = userCategoryEditDTO.Categories != null ? LocalizationIdentifierMapper.Map(userCategoryEditDTO.Categories) : null,
                Permission = permissionBL.GetPermissionById(userCategoryEditDTO.PermissionId)
            };

            return userCategory;
        }

        public static EditUserCategoryDTO Map(UserCategory userCategory)
        {
            if (userCategory == null)
            {
                return null;
            }
            EditUserCategoryDTO userCategoryEditDTO = new EditUserCategoryDTO()
            {
                Id = userCategory.Id,
                Categories = userCategory.CategoryName.Localizations != null ? LocalizationIdentifierMapper.Map(userCategory.CategoryName.Localizations) : null,
                PermissionId = userCategory.Permission.Id
            };

            return userCategoryEditDTO;
        }

        public static List<UserCategoryDTO> Map(IList<UserCategory> userCategories, string cultureName)
        {
            if (userCategories == null || !userCategories.Any())
            {
                return null;
            }
            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
            List<UserCategoryDTO> userCategoryDTOs = userCategories.Select(userCategory => new UserCategoryDTO()
            {
                Id = userCategory.Id,
                CategoryText = userCategory.LocalName,
                PermissionText = userCategory.Permission.Name.Localizations?.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text,
                Categories = userCategory.CategoryName?.Localizations != null ? LocalizationIdentifierMapper.Map(userCategory.CategoryName.Localizations) : null

            }).ToList();

            return userCategoryDTOs;
        }

    }
}
