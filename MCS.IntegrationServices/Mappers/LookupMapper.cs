using MCS.DTO;
using MCS.IntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Mappers
{
    public static class LookupMapper
    {
        public static LookupVM Map(LookupDTO lookupDTO)
        {
            if (lookupDTO != null)
            {
                LookupVM lookupVM = new LookupVM()
                {
                    Id = lookupDTO.Id,
                    CategoryId = lookupDTO.CategoryId,
                    EnumReference = lookupDTO.EnumReference,
                    IsActive = lookupDTO.IsActive,
                    // Localizations = LookupLocalizationMapper.Map(lookupDTO.Localizations),
                    Sort = lookupDTO.Sort,
                    Text = lookupDTO.Text
                };

                return lookupVM;
            }
            return new LookupVM();
        }
        public static LookupDTO Map(LookupVM lookupVM)
        {
            if (lookupVM != null)
            {
                LookupDTO lookupDTO = new LookupDTO()
                {
                    Id = lookupVM.Id,
                    CategoryId = lookupVM.CategoryId,
                    EnumReference = lookupVM.EnumReference,
                    IsActive = lookupVM.IsActive,
                    // Localizations = LookupLocalizationMapper.Map(lookupVM.Localizations),
                    Sort = lookupVM.Sort,
                    Text = lookupVM.Text
                };

                return lookupDTO;
            }
            return new LookupDTO();
        }
        public static List<LookupDTO> Map(IList<LookupVM> lookupVMs)
        {
            if (lookupVMs == null || !lookupVMs.Any())
            {
                return new List<LookupDTO>();
            }
            List<LookupDTO> lookupDTOs = lookupVMs
                .Select(lookupVM => new LookupDTO()
                {
                    Id = lookupVM.Id,
                    CategoryId = lookupVM.CategoryId,
                    EnumReference = lookupVM.EnumReference,
                    IsActive = lookupVM.IsActive,
                    // Localizations = LookupLocalizationMapper.Map(lookupVM.Localizations),
                    Sort = lookupVM.Sort,
                    Text = lookupVM.Text
                }).ToList();

            return lookupDTOs;
        }
        public static List<LookupVM> Map(IList<LookupDTO> lookupDTOs)
        {
            if (lookupDTOs == null || !lookupDTOs.Any())
            {
                return new List<LookupVM>();
            }
            List<LookupVM> lookupVMs = lookupDTOs
                .Select(lookupVM => new LookupVM()
                {
                    Id = lookupVM.Id,
                    CategoryId = lookupVM.CategoryId,
                    EnumReference = lookupVM.EnumReference,
                    IsActive = lookupVM.IsActive,
                    // Localizations = LookupLocalizationMapper.Map(lookupVM.Localizations),
                    Sort = lookupVM.Sort,
                    Text = lookupVM.Text
                }).ToList();

            return lookupVMs;
        }
    }
}