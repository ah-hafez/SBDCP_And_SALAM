using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class CityMapper
    {
        public static CityDTO Map(City city)
        {
            if (city == null)
            {
                return null;
            }
            return new CityDTO
            {
                Id = city.Id,
                CityId = city.CityId,
                Description = city.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(city.LocalizationIdentifier.Localizations) : null
            };
        }
        public static List<CityDTO> Map(List<City> cities)
        {
            if (cities == null)
            {
                return null;
            }
            return cities.Select(c => Map(c)).ToList();
        }
    }
}