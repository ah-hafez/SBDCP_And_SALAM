using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class LookupLocalizationMapper
    {
        public static List<LookupLocalizationVM> Map(IList<LookupLocalizationDTO> lookupLocalizationDTOs)
        {
            if (lookupLocalizationDTOs == null || !lookupLocalizationDTOs.Any())
            {
                return new List<LookupLocalizationVM>();
            }
            List<LookupLocalizationVM> lookupLocalizationVMs = lookupLocalizationDTOs
                .Select(lookupLocalizationDTO => new LookupLocalizationVM()
                { 
                    Id = lookupLocalizationDTO.Id,
                    CultureId = lookupLocalizationDTO.CultureId,
                    CultureName = lookupLocalizationDTO.CultureName,
                    LookupId = lookupLocalizationDTO.LookupId,
                    Text = lookupLocalizationDTO.Text
                }).ToList();

            return lookupLocalizationVMs;
        }
        public static List<LookupLocalizationDTO> Map(IList<LookupLocalizationVM> lookupLocalizationVMs)
        {
            if (lookupLocalizationVMs == null || !lookupLocalizationVMs.Any())
            {
                return new List<LookupLocalizationDTO>();
            }
            List<LookupLocalizationDTO> lookupLocalizationDTOs = lookupLocalizationVMs
                .Select(lookupLocalizationVM => new LookupLocalizationDTO()
                { 
                    Id = lookupLocalizationVM.Id,
                    CultureId = lookupLocalizationVM.CultureId,
                    CultureName = lookupLocalizationVM.CultureName,
                    LookupId = lookupLocalizationVM.LookupId,
                    Text = lookupLocalizationVM.Text
                }).ToList();

            return lookupLocalizationDTOs;
        }
    }
}