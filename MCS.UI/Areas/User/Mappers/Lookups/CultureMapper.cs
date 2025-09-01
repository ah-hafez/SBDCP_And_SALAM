using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class CultureMapper
    {
        public static List<CultureVM> Map(IList<CultureDTO> cultureDTOs)
        {
            if (cultureDTOs == null || !cultureDTOs.Any())
            {
                return new List<CultureVM>();
            }

            List<CultureVM> cultureVMs = cultureDTOs
                .Select(cultureDTO => new CultureVM()
                { 
                    Id = cultureDTO.Id,
                    LocalName = cultureDTO.LocalName,
                    ShortName = cultureDTO.ShortName
                }).ToList();

            return cultureVMs;
        }
        public static List<CultureDTO> Map(IList<CultureVM> cultureVMs)
        {
            if (cultureVMs == null || !cultureVMs.Any())
            {
                return new List<CultureDTO>();
            }
            List<CultureDTO> cultureDTOs = cultureVMs
                .Select(cultureVM => new CultureDTO()
                { 
                    Id = cultureVM.Id,
                    LocalName = cultureVM.LocalName,
                    ShortName = cultureVM.ShortName
                }).ToList();

            return cultureDTOs;
        }
        public static CultureDTO Map(CultureVM cultureVM)
        {
            if (cultureVM != null)
            {
                CultureDTO cultureDTO = new CultureDTO()
                {
                    Id = cultureVM.Id,
                    LocalName = cultureVM.LocalName,
                    ShortName = cultureVM.ShortName
                };

                return cultureDTO;
            }
            return new CultureDTO();
        }
        public static CultureVM Map(CultureDTO cultureDTO)
        {
            if (cultureDTO != null)
            {
                CultureVM cultureVM = new CultureVM()
                {
                    Id = cultureDTO.Id,
                    LocalName = cultureDTO.LocalName,
                    ShortName = cultureDTO.ShortName
                };

                return cultureVM;
            }
            return new CultureVM();
        }
    }
}