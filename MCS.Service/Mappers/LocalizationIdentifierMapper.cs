using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class LocalizationIdentifierMapper
    {
        public static LocalizationIdentifier Map(List<LocalizationDTO> localizationDTOs)
        {
            if (localizationDTOs == null || !localizationDTOs.Any())
            {
                return null;
            }
            LocalizationIdentifier identifier = new LocalizationIdentifier();
            IList<Localization> localizations = new List<Localization>();

            //ICommonBL commonBL = IoC.Resolve<ICommonBL>();

            foreach (LocalizationDTO localizationDTO in localizationDTOs)
            {
                Localization localization = new Localization()
                {
                    Id = localizationDTO.Id,
                    LocalizationIdentifier = identifier,
                    Text = localizationDTO.Text,
                    CultureId = localizationDTO.CultureId// = commonBL.GetCultureById(localizationDTO.CultureId)
                };

                localizations.Add(localization);
            }

            identifier.Localizations = localizations;

            return identifier;
        }

        public static TenantLocalizationIdentifier MapTenant(List<LocalizationDTO> localizationDTOs)
        {
            if (localizationDTOs == null || !localizationDTOs.Any())
            {
                return null;
            }
            TenantLocalizationIdentifier identifier = new TenantLocalizationIdentifier();
            IList<TenantLocalization> localizations = new List<TenantLocalization>();

            foreach (LocalizationDTO localizationDTO in localizationDTOs)
            {
                TenantLocalization localization = new TenantLocalization()
                {
                    Id = localizationDTO.Id,
                    LocalizationIdentifier = identifier,
                    Text = localizationDTO.Text,
                    CultureId = localizationDTO.CultureId
                };

                localizations.Add(localization);
            }

            identifier.Localizations = localizations;

            return identifier;
        }

        public static List<LocalizationDTO> Map(IList<Localization> localizations)
        {
            if (localizations == null || !localizations.Any())
            {
                return null;
            }
            List<LocalizationDTO> localizationDTOs = new List<LocalizationDTO>();

            //ICommonBL commonBL = IoC.Resolve<ICommonBL>();

            foreach (Localization localization in localizations)
            {
                //Culture culture = commonBL.GetCultureById(localization.Culture.Id);

                LocalizationDTO localizationDTO = new LocalizationDTO()
                {
                    Id = localization.Id,
                    Text = localization.Text,
                    CultureId = localization.Culture.Id,
                    CultureName = localization.Culture.ShortName
                };

                localizationDTOs.Add(localizationDTO);
            }

            return localizationDTOs;
        }

        public static List<Localization> Maps(IList<LocalizationDTO> localizationDTOs)
        {
            if (localizationDTOs == null || !localizationDTOs.Any())
            {
                return null;
            }
            List<Localization> localizations = new List<Localization>();

            foreach (LocalizationDTO localizationDTO in localizationDTOs)
            {
                Localization localization = new Localization()
                {
                    Id = localizationDTO.Id,
                    Text = localizationDTO.Text,
                    CultureId = localizationDTO.CultureId,
                    Culture = new Culture
                    {
                        Id = localizationDTO.CultureId,
                        ShortName = localizationDTO.CultureName

                    }
                };

                localizations.Add(localization);
            }

            return localizations;
        }

        public static List<LocalizationDTO> MapTenant(IList<TenantLocalization> localizations)
        {
            if (localizations == null || !localizations.Any())
            {
                return null;
            }
            List<LocalizationDTO> localizationDTOs = new List<LocalizationDTO>();

            foreach (TenantLocalization localization in localizations)
            {
                LocalizationDTO localizationDTO = new LocalizationDTO()
                {
                    Id = localization.Id,
                    Text = localization.Text,
                    CultureId = localization.Culture.Id,
                    CultureName = localization.Culture.ShortName
                };

                localizationDTOs.Add(localizationDTO);
            }

            return localizationDTOs;
        }

        public static List<TenantLocalization> MapTenants(IList<LocalizationDTO> localizationDTOs)
        {
            if (localizationDTOs == null || !localizationDTOs.Any())
            {
                return null;
            }
            List<TenantLocalization> localizations = new List<TenantLocalization>();


            foreach (LocalizationDTO localizationDTO in localizationDTOs)
            {
                TenantLocalization tenantLocalization = new TenantLocalization()
                {
                    Id = localizationDTO.Id,
                    Text = localizationDTO.Text,
                    CultureId = localizationDTO.CultureId,
                    Culture = new TenantCulture
                    {
                        Id = localizationDTO.CultureId,
                        ShortName = localizationDTO.CultureName,

                    }
                };

                localizationDTOs.Add(localizationDTO);
            }

            return localizations;
        }
    }
}
