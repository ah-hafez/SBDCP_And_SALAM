using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class LocalizationMapper
    {
        public static List<LocalizationVM> Map(IList<LocalizationDTO> LocalizationDTOs)
        {
            if (LocalizationDTOs == null || !LocalizationDTOs.Any())
            {
                return new List<LocalizationVM>();
            }
            List<LocalizationVM> localizationVMs = LocalizationDTOs
                .Select(LocalizationDTO => new LocalizationVM()
                { 
                    Id = LocalizationDTO.Id,
                    CultureId = LocalizationDTO.CultureId,
                    CultureName = LocalizationDTO.CultureName,
                    Text = LocalizationDTO.Text
                }).ToList();

            return localizationVMs;
        }
        public static List<LocalizationDTO> Map(IList<LocalizationVM> LocalizationVMs)
        {
            if (LocalizationVMs == null || !LocalizationVMs.Any())
            {
                return new List<LocalizationDTO>();
            }
            List<LocalizationDTO> LocalizationDTOs = LocalizationVMs
                .Select(LocalizationVM => new LocalizationDTO
                {
                    Id = LocalizationVM.Id,
                    CultureId = LocalizationVM.CultureId,
                    CultureName = LocalizationVM.CultureName,
                    Text = LocalizationVM.Text
                }).ToList();
            return LocalizationDTOs;
        }
        public static LocalizationDTO Map(LocalizationVM localizationVM)
        {
            if (localizationVM != null)
            {
                LocalizationDTO localizationDTO = new LocalizationDTO()
                {
                    Id = localizationVM.Id,
                    CultureId = localizationVM.CultureId,
                    CultureName = localizationVM.CultureName,
                    Text = localizationVM.Text
                };
                return localizationDTO;
            }
            return new LocalizationDTO();
        }
        public static LocalizationVM Map(LocalizationDTO localizationDTO)
        {
            if (localizationDTO != null)
            {
                LocalizationVM localizationVM = new LocalizationVM()
                {
                    Id = localizationDTO.Id,
                    CultureId = localizationDTO.CultureId,
                    CultureName = localizationDTO.CultureName,
                    Text = localizationDTO.Text
                };
                return localizationVM;
            }
            return new LocalizationVM();
        }
    }
}