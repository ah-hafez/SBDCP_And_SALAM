using MCS.Domain;
using MCS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.Service.Mappers
{
    public class ThemeMapper
    {
        public static List<ThemeDTO> Map(IList<Theme> theme ,string ShortName)
        {
            if (theme == null || !theme.Any())
            {
                return null;
            }
            List<ThemeDTO> themeDTOs = theme
                .Select(themeDTO => new ThemeDTO()
                {
                    Id = themeDTO.Id,
                    LocalName = themeDTO.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == ShortName).FirstOrDefault().Text,
                    Path=themeDTO.Path
                }).ToList();

            return themeDTOs;
        }
    }
}