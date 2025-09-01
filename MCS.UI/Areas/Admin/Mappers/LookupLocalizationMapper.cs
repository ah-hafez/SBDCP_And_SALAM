using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class LookupLocalizationMapper
    {
        public static List<LookupLocalizationDTO> Map(IList<LookupLocalizationVM> lookupLocalizationVMs)
        {
            if (lookupLocalizationVMs == null || !lookupLocalizationVMs.Any())
            { return null; }
            List<LookupLocalizationDTO> lookupLocalizationDTOs = lookupLocalizationVMs
                .Select(b => new LookupLocalizationDTO
                { 
                    CultureId = b.CultureId,
                    CultureName = b.CultureName,
                    Id = b.Id,
                    LookupId = b.LookupId,
                    Text = b.Text
                }).ToList();
            return lookupLocalizationDTOs;
        }
        public static List<LookupLocalizationVM> Map(IList<LookupLocalizationDTO> lookupLocalizationDTOs)
        {
            if (lookupLocalizationDTOs == null || !lookupLocalizationDTOs.Any())
            { return null; }
            List<LookupLocalizationVM> lookupLocalizationVMs = lookupLocalizationDTOs
                .Select(b => new LookupLocalizationVM
                {
                     
                    CultureId = b.CultureId,
                    CultureName = b.CultureName,
                    Id = b.Id,
                    LookupId = b.LookupId,
                    Text = b.Text
                }).ToList();
            return lookupLocalizationVMs;
        }

    }
}