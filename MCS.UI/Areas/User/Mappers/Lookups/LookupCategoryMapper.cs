using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class LookupCategoryMapper
    {
        public static List<LookupCategoryVM> Map(IList<LookupCategoryDTO> lookupCategoryDTOs)
        {
            if (lookupCategoryDTOs == null || !lookupCategoryDTOs.Any())
            {
                return new List<LookupCategoryVM>();
            }
            List<LookupCategoryVM> lookupCategoryVMs = lookupCategoryDTOs
                .Select(lookupCategoryDTO => new LookupCategoryVM()
                { 
                    Id = lookupCategoryDTO.Id,
                    Culture = CultureMapper.Map(lookupCategoryDTO.Culture),
                    Text = lookupCategoryDTO.Text
                }).ToList();
            return lookupCategoryVMs;
        }
        public static List<LookupCategoryDTO> Map(IList<LookupCategoryVM> lookupCategoryVMs)
        {
            if (lookupCategoryVMs == null || !lookupCategoryVMs.Any())
            {
                return new List<LookupCategoryDTO>();
            }
            List<LookupCategoryDTO> lookupCategoryDTOs = lookupCategoryVMs
                .Select(lookupCategoryVM => new LookupCategoryDTO()
                { 
                    Id = lookupCategoryVM.Id,
                    Culture = CultureMapper.Map(lookupCategoryVM.Culture),
                    Text = lookupCategoryVM.Text
                }).ToList();
            return lookupCategoryDTOs;
        }
    }
}