using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public class ThemeMapper
    {
        public static List<ThemeVM> Map(IList<ThemeDTO> themeDTOs )
        {
            if (themeDTOs == null || !themeDTOs.Any())
            {
                return new List<ThemeVM>();
            }

            List<ThemeVM> themeVMs = themeDTOs
                .Select(themeDTO => new ThemeVM()
                {
                    Id = themeDTO.Id,
                    LocalName = themeDTO.LocalName,
                    Path=themeDTO.Path
                    
                }).ToList();

            return themeVMs;
        }
        public static List<ThemeDTO> Map(IList<ThemeVM> themeVMs)
        {
            if (themeVMs == null || !themeVMs.Any())
            {
                return new List<ThemeDTO>();
            }
            List<ThemeDTO> themeDTOs = themeVMs
                .Select(themeVM => new ThemeDTO()
                {
                    Id = themeVM.Id,
                    LocalName =themeVM.LocalName,
                    Path = themeVM.Path
                }).ToList();

            return themeDTOs;
        }
        public static ThemeDTO Map(ThemeVM themeVM)
        {
            if (themeVM != null)
            {
                ThemeDTO themeDTO = new ThemeDTO()
                {
                    Id = themeVM.Id,
                    LocalName = themeVM.LocalName,
                    Path = themeVM.Path
                };

                return themeDTO;
            }
            return new ThemeDTO();
        }
        public static ThemeVM Map(ThemeDTO themeDTO)
        {
            if (themeDTO != null)
            {
                ThemeVM themeVM = new ThemeVM()
                {
                    Id = themeDTO.Id,
                    LocalName = themeDTO.LocalName,
                    Path = themeDTO.Path
                };

                return themeVM;
            }
            return new ThemeVM();
        }
    }
}
