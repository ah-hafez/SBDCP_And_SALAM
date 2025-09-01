using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class UserTrayPreferenceInfoMapper
    {
        public static List<UserTrayPreferencesDTO> Map(IList<UserTrayPreferenceInfo> userTrayPreferenceInfos)
        {
            if (userTrayPreferenceInfos == null || !userTrayPreferenceInfos.Any())
            {
                return null;
            }
            List<UserTrayPreferencesDTO> userTrayPreferencesDTOs = userTrayPreferenceInfos
                .Select(userTrayPreferenceInfo => new UserTrayPreferencesDTO()
                {
                    Id = userTrayPreferenceInfo.TrayId,
                    Name = userTrayPreferenceInfo.TrayName,
                    IsSelected = userTrayPreferenceInfo.IsSelected
                }).ToList();
            return userTrayPreferencesDTOs;
        }

        public static List<UserTrayPreferenceInfo> Map(IList<UserTrayPreferencesDTO> userTrayPreferencesDTOs)
        {
            if (userTrayPreferencesDTOs == null || !userTrayPreferencesDTOs.Any())
            {
                return null;
            }
            List<UserTrayPreferenceInfo> UserTrayPreferenceInfo = userTrayPreferencesDTOs
                .Select(userTrayPreferencesDTO => new UserTrayPreferenceInfo()
                {
                    TrayId = userTrayPreferencesDTO.Id,
                    TrayName = userTrayPreferencesDTO.Name,
                    IsSelected = userTrayPreferencesDTO.IsSelected
                }).ToList();
            return UserTrayPreferenceInfo;
        }
    }
}