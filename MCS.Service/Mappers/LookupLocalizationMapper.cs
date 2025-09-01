using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class LookupLocalizationMapper
    {
        public static List<LookupLocalization> Map(IList<LookupLocalizationDTO> lookupLocalizationDTOs)
        {
            if (lookupLocalizationDTOs == null || !lookupLocalizationDTOs.Any())
            {
                return null;
            }
            List<LookupLocalization> lookupLocalizations = new List<LookupLocalization>();

            ICommonBL commonBL = IoC.Resolve<ICommonBL>();

            foreach (LookupLocalizationDTO lookupLocalizationDTO in lookupLocalizationDTOs)
            {
                LookupLocalization lookupLocalization = new LookupLocalization()
                {
                    Id = lookupLocalizationDTO.Id,
                    Text = lookupLocalizationDTO.Text
                };

                lookupLocalization.Culture = commonBL.GetCultureById(lookupLocalizationDTO.CultureId);

                lookupLocalizations.Add(lookupLocalization);
            }

            return lookupLocalizations;
        }

        public static List<LookupLocalizationDTO> Map(IList<LookupLocalization> lookupLocalizations)
        {
            if (lookupLocalizations == null || !lookupLocalizations.Any())
            {
                return null;
            }
            List<LookupLocalizationDTO> lookupLocalizationDTOs = new List<LookupLocalizationDTO>();

            foreach (LookupLocalization lookupLocalization in lookupLocalizations)
            {
                if (lookupLocalization.Culture != null)
                {
                    LookupLocalizationDTO localizationDTO = new LookupLocalizationDTO();

                    localizationDTO.Id = lookupLocalization.Id;
                    localizationDTO.CultureId = lookupLocalization.Culture.Id;
                    localizationDTO.Text = lookupLocalization.Text;
                    localizationDTO.CultureName = lookupLocalization.Culture.ShortName;

                    lookupLocalizationDTOs.Add(localizationDTO);
                }
            }

            return lookupLocalizationDTOs;
        }
    }
}