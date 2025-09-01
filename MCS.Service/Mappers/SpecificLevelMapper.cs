using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class SpecificLevelMapper
    {
        public static SpecificLevel Map(SpecificLevelAddDTO specificLevelAddDTO)
        {
            if (specificLevelAddDTO == null)
                return null;
            TransactionCategories transactionCategories = 
                TransactionCategoryMapper.Map(specificLevelAddDTO.TransactionCategories);

            SpecificLevel specificLevel = new SpecificLevel()
             {
                 TransactionCategories = transactionCategories,
                //SpecificLevelList = SpecificLevelListMapper.Map(specificLevelAddDTO.List),
                LocalizationIdentifier = specificLevelAddDTO.Description !=null ? LocalizationIdentifierMapper.Map(specificLevelAddDTO.Description):null
             };

            return specificLevel;
        }

        public static SpecificLevel Map(SpecificLevelEditDTO specificLevelEditDTO)
        {
            if (specificLevelEditDTO == null)
                return null;
            TransactionCategories transactionCategories = 
                TransactionCategoryMapper.Map(specificLevelEditDTO.TransactionCategories);

            SpecificLevel specificLevel = new SpecificLevel()
            {
                Id = specificLevelEditDTO.Id, 
                TransactionCategories = transactionCategories,
                LocalizationIdentifier = specificLevelEditDTO.Description !=null ? LocalizationIdentifierMapper.Map(specificLevelEditDTO.Description): null,
                //SpecificLevelList = SpecificLevelListMapper.Map(specificLevelEditDTO.List)
            };

            return specificLevel;
        }

        public static SpecificLevelEditDTO Map(SpecificLevel specificLevel, string cultureName)
        {
            if (specificLevel == null)
                return null;
            SpecificLevelEditDTO specificLevelEditDTO = new SpecificLevelEditDTO()
            {
                Id = specificLevel.Id,
                TransactionCategories = TransactionCategoryMapper.Map(specificLevel.TransactionCategories, cultureName),
                //List = SpecificLevelListMapper.Map(specificLevel.SpecificLevelList),
                Description = new List<LocalizationDTO>()
            };

            foreach (Localization localization in specificLevel.LocalizationIdentifier.Localizations)
            {
                if (localization.Culture != null)
                {
                    LocalizationDTO localizationDTO = new LocalizationDTO();

                    localizationDTO.Id = localization.Id;
                    localizationDTO.CultureId = localization.Culture.Id;
                    localizationDTO.Text = localization.Text;
                    localizationDTO.CultureName = localization.Culture.ShortName;
                    specificLevelEditDTO.Description.Add(localizationDTO);
                }
            }

            return specificLevelEditDTO;
        }

        public static List<SpecificLevelDTO> Map(IList<SpecificLevel> SpecificLevels, string cultureName)
        {
            List<SpecificLevelDTO> specificLevelDTOs = new List<SpecificLevelDTO>();

            foreach (SpecificLevel specificLevel in SpecificLevels)
            {
                specificLevelDTOs.Add(SpecificLevelMapper.MapSpecificLevel(specificLevel, cultureName));
            }

            return specificLevelDTOs;
        }
        public static List<SpecificLevel> Map(IList<SpecificLevelDTO> specificLevelDTOs, string cultureName)
        {
            if (specificLevelDTOs == null || !specificLevelDTOs.Any())
            {
                return null;
            }
            List<SpecificLevel> specificLevels = new List<SpecificLevel>();

            foreach (SpecificLevelDTO specificLevelDTO in specificLevelDTOs)
            {
                specificLevels.Add(SpecificLevelMapper.MapSpecificLevel(specificLevelDTO, cultureName));
            }

            return specificLevels;
        }


        private static SpecificLevelDTO MapSpecificLevel(SpecificLevel specificLevel, string cultureName)
        {
            if (specificLevel == null)
                return null;
            SpecificLevelDTO specificLevelDTO = new SpecificLevelDTO()
            {
                Id = specificLevel.Id,
                LocalName = specificLevel.Text,
                TransactionCategories = TransactionCategoryMapper.Map(specificLevel.TransactionCategories, cultureName)            };

            if (specificLevel.LocalizationIdentifier != null)
            {
                specificLevelDTO.Description = new List<LocalizationDTO>();

                foreach (Localization localization in specificLevel.LocalizationIdentifier.Localizations)
                {
                    if (localization.Culture != null)
                    {
                        LocalizationDTO localizationDTO = new LocalizationDTO();

                        localizationDTO.Id = localization.Id;
                        localizationDTO.CultureId = localization.Culture.Id;
                        localizationDTO.Text = localization.Text;
                        localizationDTO.CultureName = localization.Culture.ShortName;
                        specificLevelDTO.Description.Add(localizationDTO);
                    }
                }
            }

            return specificLevelDTO;
        }

        private static SpecificLevel MapSpecificLevel(SpecificLevelDTO specificLevelDTO, string cultureName)
        {
            if (specificLevelDTO == null)
                return null;
            SpecificLevel specificLevel = new SpecificLevel()
            {
                Id = specificLevelDTO.Id,
                Text = specificLevelDTO.LocalName,
                TransactionCategories = TransactionCategoryMapper.Map(specificLevelDTO.TransactionCategories)
                
            };

            if (specificLevelDTO.Description != null)
            {
                specificLevel.LocalizationIdentifier.Localizations = new List<Localization>();

                foreach (LocalizationDTO localizationDTO in specificLevelDTO.Description)
                {
                    if (localizationDTO.CultureName != null)
                    {
                        Localization localization = new Localization();

                        localization.Id = localizationDTO.Id;
                        localization.CultureId = localizationDTO.CultureId;
                        localization.Text = localizationDTO.Text;
                        localization.Culture.ShortName = localizationDTO.CultureName;
                        localization.LocalizationIdentifier.Localizations.Add(localization);
                    }
                }
            }

            return specificLevel;
        }
    }
}