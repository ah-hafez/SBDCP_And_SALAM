using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.TenantsAdmin.Models.LookupsVM;

namespace MCS.UI.TenantsAdmin.Mappers.Lookups
{
    public class LocalizationMapper
    {
        public static List<LocalizationVM> Map(IList<LocalizationDTO> localizations)
        {

            List<LocalizationVM> localizationVMs = localizations
                .Select(localization => new LocalizationVM
                {
                    CultureId = localization.CultureId,
                    CultureName= localization.CultureName,
                    Id= localization.Id,
                    Text= localization.Text
                }).ToList();
            return localizationVMs;
        }

    }
}