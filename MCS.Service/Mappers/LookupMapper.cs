using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class LookupMapper
    {
        public static Lookup Map(LookupDTO lookupDTO)
        {
            if (lookupDTO == null)
                return null;

            Lookup lookup = new Lookup()
            {
                Id = lookupDTO.Id,
                IsActive = lookupDTO.IsActive,
                Sort = lookupDTO.Sort,
                CategoryId = lookupDTO.CategoryId,
                EnumReference = lookupDTO.EnumReference,
                Text = lookupDTO.Text,
                Localizations = LookupLocalizationMapper.Map(lookupDTO.Localizations)
            };

            return lookup;
        }

        public static LookupDTO Map(Lookup lookup)
        {
            if (lookup == null)
                return null;
            if (lookup == null)
            {
                return null;
            }

            LookupDTO lookupDTO = new LookupDTO()
            {
                Id = lookup.Id,
                IsActive = lookup.IsActive,
                Sort = lookup.Sort,
                CategoryId = lookup.CategoryId,
                EnumReference = lookup.EnumReference,
                Text = lookup.Text,
            };

            if (lookup.Localizations != null)
                lookupDTO.Localizations = LookupLocalizationMapper.Map(lookup.Localizations);

            return lookupDTO;
        }

        public static List<LookupDTO> Map(IList<Lookup> lookups)
        {
            if (lookups == null || !lookups.Any())
            {
                return null;
            }
            List<LookupDTO> lookupDTOs = lookups.Select(lookupDTO => new LookupDTO()
            {
                Id = lookupDTO.Id,
                IsActive = lookupDTO.IsActive,
                Sort = lookupDTO.Sort,
                CategoryId = lookupDTO.CategoryId,
                EnumReference = lookupDTO.EnumReference,
                Text = lookupDTO.Text,
                Localizations = LookupLocalizationMapper.Map(lookupDTO.Localizations)
            }).ToList();



            return lookupDTOs;
        }

        public static List<Lookup> Map(IList<LookupDTO> lookupDTOs)
        {
            if (lookupDTOs == null || !lookupDTOs.Any())
            {
                return null;
            }
            List<Lookup> lookups = lookupDTOs.Select(lookup => new Lookup()
            {
                Id = lookup.Id,
                IsActive = lookup.IsActive,
                Sort = lookup.Sort,
                CategoryId = lookup.CategoryId,
                EnumReference = lookup.EnumReference,
                Text = lookup.Text,
                Localizations = LookupLocalizationMapper.Map(lookup.Localizations)
            }).ToList(); 
            return lookups;
        }
    }
}