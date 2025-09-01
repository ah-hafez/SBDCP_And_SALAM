using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class LookupMapper
    {
        public static LookupDTO Map(LookupVM lookupVMs)
        {
            if (lookupVMs != null)
            {
                return new LookupDTO()
                { 
                    Id = lookupVMs.Id,
                    CategoryId = lookupVMs.CategoryId,
                    IsActive = lookupVMs.IsActive,
                    EnumReference = lookupVMs.EnumReference,
                    Sort = lookupVMs.Sort,
                    Localizations = LookupLocalizationMapper.Map(lookupVMs.Localizations),
                    Text = lookupVMs.Text
                };
            }
            return null;
        }
        
        public static LookupVM Map(LookupDTO lookupVMs)
        {
            if (lookupVMs != null)
            {
                return new LookupVM()
                {
                    Id = lookupVMs.Id,
                    CategoryId = lookupVMs.CategoryId,
                    IsActive = lookupVMs.IsActive,
                    EnumReference = lookupVMs.EnumReference,
                    Sort = lookupVMs.Sort,
                    Localizations = LookupLocalizationMapper.Map(lookupVMs.Localizations),
                    Text = lookupVMs.Text
                };
            }
            return null;
        }
        public static List<LookupDTO> Map(IList<LookupVM> lookupVMs)
        {
            if (lookupVMs == null || !lookupVMs.Any())
            { return null; }
            List<LookupDTO> lookupDTOs = lookupVMs
                .Select(b => new LookupDTO
                {
                    Id = b.Id,
                    CategoryId = b.CategoryId,
                    IsActive = b.IsActive,
                    EnumReference = b.EnumReference,
                    Sort = b.Sort,
                    Localizations = LookupLocalizationMapper.Map(b.Localizations),
                    Text = b.Text
                }).ToList();
            return lookupDTOs;
        }
        public static List<LookupVM> Map(IList<LookupDTO> lookupDTOs)
        {
            if (lookupDTOs == null || !lookupDTOs.Any())
            { return null; }
            List<LookupVM> lookupVMs = lookupDTOs
                .Select(b => new LookupVM
                {
                    Id = b.Id,
                    CategoryId = b.CategoryId,
                    IsActive = b.IsActive,
                    EnumReference = b.EnumReference,
                    Sort = b.Sort,
                    Localizations = LookupLocalizationMapper.Map(b.Localizations),
                    Text = b.Text
                }).ToList();
            return lookupVMs;
        }
    }
}