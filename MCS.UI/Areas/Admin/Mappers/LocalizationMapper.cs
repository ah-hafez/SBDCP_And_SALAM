using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class LocalizationMapper
    {
        public static List<LocalizationDTO> Map(IList<LocalizationVM> localizationVMs)
        {
            if (localizationVMs == null || !localizationVMs.Any())
            { return null; }
            List<LocalizationDTO> localizationDTOs = localizationVMs
                .Select(b => new LocalizationDTO
                {  
                    CultureId = b.CultureId,
                    CultureName = b.CultureName,
                    Id = b.Id,
                    Text = b.Text
                }).ToList();
            return localizationDTOs;
        }
        public static List<LocalizationVM> Map(IList<LocalizationDTO> localizationDTOs)
        {
            if (localizationDTOs == null || !localizationDTOs.Any())
            { return null; }
            List<LocalizationVM> localizationVMs = localizationDTOs
                .Select(b => new LocalizationVM
                {  
                    CultureId = b.CultureId,
                    CultureName = b.CultureName,
                    Id = b.Id,
                    Text = b.Text 
                }).ToList();
            return localizationVMs;
        }

    }
}