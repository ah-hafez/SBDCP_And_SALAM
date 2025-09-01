using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.UserPreferences;

namespace MCS.UI.Areas.User.Mappers.UserPreferences
{
    public static class UserTrayPreferencesMapper
    {
        public static List<UserTrayPreferencesVM> Map(IList<UserTrayPreferencesDTO> userTrayPreferencesDTOs)
        {
            if (userTrayPreferencesDTOs == null || !userTrayPreferencesDTOs.Any())
            {
                return new List<UserTrayPreferencesVM>();
            }
            List<UserTrayPreferencesVM> userTrayPreferencesVMs = userTrayPreferencesDTOs
                .Select(userTrayPreferencesDTO => new UserTrayPreferencesVM()
                {
                    Id = userTrayPreferencesDTO.Id,
                    IsSelected = userTrayPreferencesDTO.IsSelected,
                    Name = userTrayPreferencesDTO.Name
                }).ToList();

            return userTrayPreferencesVMs;
        }
        public static List<UserTrayPreferencesDTO> Map(IList<UserTrayPreferencesVM> userTrayPreferencesVMs)
        {
            if (userTrayPreferencesVMs == null || !userTrayPreferencesVMs.Any())
            {
                return new List<UserTrayPreferencesDTO>();
            }
            List<UserTrayPreferencesDTO> userTrayPreferencesDTOs = userTrayPreferencesVMs
                .Select(userTrayPreferencesVM => new UserTrayPreferencesDTO()
                {
                    Id = userTrayPreferencesVM.Id,
                    IsSelected = userTrayPreferencesVM.IsSelected,
                    Name = userTrayPreferencesVM.Name
                }).ToList();

            return userTrayPreferencesDTOs;
        }
    }
}