using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class CultureMapper
    {
        public static List<CultureDTO> Map(IList<Culture> cultures)
        {
            if (cultures == null || !cultures.Any())
            {
                return null;
            }
            List<CultureDTO> cultureDTOs = cultures
                .Select(cultureDTO => new CultureDTO()
            {
                Id = cultureDTO.Id,
                ShortName = cultureDTO.ShortName,
                LocalName = cultureDTO.Name.Localizations.Where(l => l.Culture.ShortName == cultureDTO.ShortName).FirstOrDefault().Text
            }).ToList() ;

            return cultureDTOs;
        }
    }
}