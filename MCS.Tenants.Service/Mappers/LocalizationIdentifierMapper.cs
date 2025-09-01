using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YESSER.NCS.MCM.Domain;
using YESSER.NCS.MCM.DTO.Tenant;

namespace YESSER.NCS.MCM.Tenants.Service.Mappers
{
    public static class LocalizationIdentifierMapper
    {
        public static TenantLocalizationIdentifier MapCulture(List<LocalizationDTO> localizationDTOs)
        {
            TenantLocalizationIdentifier identifier = new TenantLocalizationIdentifier();
            IList<TenantLocalization> localizations = new List<TenantLocalization>();


            foreach (LocalizationDTO localizationDTO in localizationDTOs)
            {
                TenantLocalization localization = new TenantLocalization()
                {
                    ID = localizationDTO.Id,
                    LocalizationIdentifier = identifier,
                    Text = localizationDTO.Text,
                    Culture = localizationDTO.Culture
                };

                localizations.Add(localization);
            }

            identifier.Localizations = localizations;

            return identifier;
        }


        public static List<LocalizationVM> Map(IList<LocalizationDTO> localizationDTOs)
        {
            List<LocalizationVM> localizationVMs = new List<LocalizationVM>();
            ICommonBL commonBL = new CommonBL();
            foreach (var localization in localizationDTOs)
            {
                LocalizationVM localizationVM = new LocalizationVM()
                {
                    Id = localization.Id,
                    Text = localization.Text,
                    CultureId = localization.CultureId,
                    CultureName = localization.CultureName
                };
                localizationVMs.Add(localizationVM);
            }
            return localizationVMs;
        }
        public static LocalizationVM Map(LocalizationDTO localizationDTO)
        {

            LocalizationVM localizationVM = new LocalizationVM()
            {
                Id = localizationDTO.Id,
                Text = localizationDTO.Text,
                CultureId = localizationDTO.CultureId,
                CultureName = localizationDTO.CultureName
            };

            return localizationVM;
        }

        public static List<LocalizationDTO> MapTenant(IList<TenantLocalization> localizations)
        {
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

        public static TenantLocalizationIdentifier MapTenant(List<LocalizationDTO> localizationDTOs)
        {
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
    }
}