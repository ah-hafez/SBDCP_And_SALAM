using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class CityMapper
    {
        public static CityVM Map(CityDTO cityDTO)
        {
            if (cityDTO == null)
            {
                return null;
            }
            return new CityVM
            {
                Id = cityDTO.Id,
                CityId = cityDTO.CityId,
                Description = LocalizationMapper.Map(cityDTO.Description)
            };
        }
        public static List<CityVM> Map(List<CityDTO> cityDTOs)
        {
            if (cityDTOs == null)
            {
                return null;
            }
            return cityDTOs.Select(c => Map(c)).ToList();
        }
    }
}