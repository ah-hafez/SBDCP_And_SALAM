using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class UserCategoryTrayMapper
    {
        public static List<UserCategoryTray> Map(EditUserCategoryTrayDTO userCategoryTrayDTO)
        {
            if (userCategoryTrayDTO == null)
            {
                return null;
            }
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            List<UserCategoryTray> userCategoriesTrays = new List<UserCategoryTray>();
            UserCategory userCategory = userManagementBL.GetUserCategoryById(userCategoryTrayDTO.UserCategoryId);

            foreach (int trayId in userCategoryTrayDTO.TraysIds)
            {
                UserCategoryTray userCategoryTray = new UserCategoryTray()
                {
                    UserCategory = userCategory,
                    Tary = TrayBaseBL.GetTrayById(trayId)

                };

                userCategoriesTrays.Add(userCategoryTray);
            }

            return userCategoriesTrays;
        }

        public static List<UserCategoryTray> Map(IList<UserCategoryTrayDTO> userCategoriesTraysDTO)
        {
            if (userCategoriesTraysDTO == null || !userCategoriesTraysDTO.Any())
            {
                return null;
            }
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            List<UserCategoryTray> userCategoriesTrays = new List<UserCategoryTray>();

            foreach (UserCategoryTrayDTO userCategoryTrayDTO in userCategoriesTraysDTO)
            {
                UserCategory userCategory = userManagementBL.GetUserCategoryById(userCategoryTrayDTO.Id);

                foreach (TrayDTO trayDTO in userCategoryTrayDTO.Trays)
                {
                    if (trayDTO.IsSelected)
                    {
                        UserCategoryTray userCategoryTray = new UserCategoryTray()
                        {
                            UserCategory = userCategory,
                            Tary = TrayBaseBL.GetTrayById(trayDTO.Id)
                        };

                        userCategoriesTrays.Add(userCategoryTray);
                    }
                }
            }

            return userCategoriesTrays;
        }

        public static List<UserCategoryTrayDTO> Map(IList<UserCategory> userCategories, string cultureName)
        {
            List<UserCategoryTrayDTO> userCategoryTrayDTOs = new List<UserCategoryTrayDTO>();

            foreach (UserCategory userCategory in userCategories)
            {
                userCategoryTrayDTOs.Add(UserCategoryTrayMapper.MapUserCategoryTray(userCategory, cultureName));
            }

            return userCategoryTrayDTOs;
        }

        public static List<UserCategoryDTO> MapUserCategories(IList<UserCategory> userCategories)
        {
            if (userCategories == null || !userCategories.Any())
            {
                return null;
            }
            List<UserCategoryDTO> userCategoryDTOs = userCategories
                .Select(userCategory => new UserCategoryDTO()
                {
                    Id = userCategory.Id,
                    CategoryText = userCategory.LocalName,
                    IsSelected = false
                }).ToList();

            return userCategoryDTOs;
        }

        public static List<UserCategory> MapUserCategories(List<UserCategoryDTO> userCategoryDTOs)
        {
            if (userCategoryDTOs == null || !userCategoryDTOs.Any())
            {
                return null;
            }
            List<UserCategory> userCategories = userCategoryDTOs
                .Select(userCategoryDTO => new UserCategory()
                {
                    Id = userCategoryDTO.Id,
                    LocalName = userCategoryDTO.CategoryText,
                }).ToList();
            return userCategories;
        }

        private static UserCategoryDTO MapUserCategory(UserCategory userCategory)
        {
            if (userCategory == null)
            {
                return null;
            }
            UserCategoryDTO userCategoryDTO = new UserCategoryDTO()
            {
                Id = userCategory.Id,
                CategoryText = userCategory.LocalName,
                IsSelected = false
            };

            if (userCategory.CategoryName != null)
            {
                userCategoryDTO.Categories = userCategory.CategoryName.Localizations != null ? LocalizationIdentifierMapper.Map(userCategory.CategoryName.Localizations) : null;
            }

            return userCategoryDTO;
        }

        private static UserCategoryTrayDTO MapUserCategoryTray(UserCategory userCategory, string cultureName)
        {
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

            IList<TrayDTO> trayDTOs = TrayMapper.Map(TrayBaseBL.GetAllTrays(cultureName));

            UserCategoryTrayDTO userCategoryTrayDTO = new UserCategoryTrayDTO()
            {
                Id = userCategory.Id,
                CategoryText = userCategory.LocalName,
                Trays = TrayMapper.Map(userManagementBL.GetUserCategoryTrays(userCategory.Id, cultureName), true),
            };

            if (userCategory.CategoryName != null)
            {
                userCategoryTrayDTO.Categories = userCategory.CategoryName.Localizations != null ? LocalizationIdentifierMapper.Map(userCategory.CategoryName.Localizations) : null;
            }

            foreach (TrayDTO trayDTO in trayDTOs)
            {
                if (userCategoryTrayDTO.Trays == null)
                {
                    break;
                }

                if (!userCategoryTrayDTO.Trays.Any(u => u.Id == trayDTO.Id))
                {
                    userCategoryTrayDTO.Trays.Add(trayDTO);
                }
            }

            return userCategoryTrayDTO;
        }
    }
}