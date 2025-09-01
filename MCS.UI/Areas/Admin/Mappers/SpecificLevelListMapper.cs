using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class SpecificLevelListMapper
    {
        public static SpecificLevelListDTO Map(SpecificListLevelVM specificLevelListVM)
        {
            if (specificLevelListVM != null)
            {
                return new SpecificLevelListDTO()
                {
                    Id = specificLevelListVM.Id,
                    Text = specificLevelListVM.Text
                };
            }
            return null;
        }
        public static SpecificListLevelVM Map(SpecificLevelListDTO specificLevelListDTO)
        {
            if (specificLevelListDTO != null)
            {
                return new SpecificListLevelVM()
                {
                    Id = specificLevelListDTO.Id,
                    Text = specificLevelListDTO.Text
                };
            }
            return null;
        }
        public static List<SpecificLevelListDTO> Map(IList<SpecificListLevelVM> specificLevelListVMs)
        {
            if (specificLevelListVMs == null || !specificLevelListVMs.Any())
            { return null; }
            List<SpecificLevelListDTO> specificLevelListDTOs = specificLevelListVMs
                .Select(b => new SpecificLevelListDTO
                { 
                    Id = b.Id,
                    Text = b.Text

                }).ToList();
            return specificLevelListDTOs;
        }
        public static List<SpecificListLevelVM> Map(IList<SpecificLevelListDTO> specificLevelListDTOs)
        {
            if (specificLevelListDTOs == null || !specificLevelListDTOs.Any())
            {
                return null;
            }
            List<SpecificListLevelVM> specificLevelListVMs = specificLevelListDTOs
                .Select(b => new SpecificListLevelVM
                { 
                    
                    Id = b.Id,
                    Text = b.Text

                }).ToList();
            return specificLevelListVMs;
        }

    }
}