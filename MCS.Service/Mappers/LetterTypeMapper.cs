using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class LetterTypeMapper
    {
        public static LetterType Map(LetterTypeAddDTO letterTypeAddDTO)
        {
            if (letterTypeAddDTO == null)
                return null;
            TransactionCategories transactionCategories = 
                TransactionCategoryMapper.Map(letterTypeAddDTO.TransactionCategories);

            LetterType letterType = new LetterType()
             {
                 IsPopularization=letterTypeAddDTO.IsPopularization,
                 TransactionCategories = transactionCategories,
                 LetterListType = LetterTypeListMapper.Map(letterTypeAddDTO.List),
                 LocalizationIdentifier = letterTypeAddDTO.Description !=null ? LocalizationIdentifierMapper.Map(letterTypeAddDTO.Description):null
             };

            return letterType;
        }

        public static LetterType Map(LetterTypeEditDTO letterTypeEditDTO)
        {
            if (letterTypeEditDTO == null)
                return null;
            TransactionCategories transactionCategories = 
                TransactionCategoryMapper.Map(letterTypeEditDTO.TransactionCategories);

            LetterType letterType = new LetterType()
            {
                Id = letterTypeEditDTO.Id,
                IsPopularization=letterTypeEditDTO.IsPopularization,
                TransactionCategories = transactionCategories,
                LocalizationIdentifier = letterTypeEditDTO.Description !=null ? LocalizationIdentifierMapper.Map(letterTypeEditDTO.Description): null,
                LetterListType = LetterTypeListMapper.Map(letterTypeEditDTO.List)
            };

            return letterType;
        }

        public static LetterTypeEditDTO Map(LetterType letterType, string cultureName)
        {
            if (letterType == null)
                return null;
            LetterTypeEditDTO letterTypeEditDTO = new LetterTypeEditDTO()
            {
                Id = letterType.Id,
                IsPopularization=letterType.IsPopularization,
                TransactionCategories = TransactionCategoryMapper.Map(letterType.TransactionCategories, cultureName),
                List = LetterTypeListMapper.Map(letterType.LetterListType),
                Description = new List<LocalizationDTO>()
            };

            foreach (Localization localization in letterType.LocalizationIdentifier.Localizations)
            {
                if (localization.Culture != null)
                {
                    LocalizationDTO localizationDTO = new LocalizationDTO();

                    localizationDTO.Id = localization.Id;
                    localizationDTO.CultureId = localization.Culture.Id;
                    localizationDTO.Text = localization.Text;
                    localizationDTO.CultureName = localization.Culture.ShortName;
                    letterTypeEditDTO.Description.Add(localizationDTO);
                }
            }

            return letterTypeEditDTO;
        }

        public static List<LetterTypeDTO> Map(IList<LetterType> letterTypes, string cultureName)
        {
            List<LetterTypeDTO> letterTypeDTOs = new List<LetterTypeDTO>();

            foreach (LetterType letterType in letterTypes)
            {
                letterTypeDTOs.Add(LetterTypeMapper.MapLetterType(letterType, cultureName));
            }

            return letterTypeDTOs;
        }
        public static List<LetterType> Map(IList<LetterTypeDTO> letterTypeDTOs, string cultureName)
        {
            if (letterTypeDTOs == null || !letterTypeDTOs.Any())
            {
                return null;
            }
            List<LetterType> letterTypes = new List<LetterType>();

            foreach (LetterTypeDTO letterTypeDTO in letterTypeDTOs)
            {
                letterTypes.Add(LetterTypeMapper.MapLetterType(letterTypeDTO, cultureName));
            }

            return letterTypes;
        }


        public static LetterTypeDTO MapLetterType(LetterType letterType, string cultureName)
        {
            if (letterType == null)
                return null;
            LetterTypeDTO letterTypeDTO = new LetterTypeDTO()
            {
                Id = letterType.Id,
                LocalName = letterType.Text,
                IsPopularization=letterType.IsPopularization,
                TransactionCategories = TransactionCategoryMapper.Map(letterType.TransactionCategories, cultureName),
                Notify = letterType.Notify,
                WithExtraField = letterType.WithExtraField
            };

            if (letterType.LocalizationIdentifier != null)
            {
                letterTypeDTO.Description = new List<LocalizationDTO>();

                foreach (Localization localization in letterType.LocalizationIdentifier.Localizations)
                {
                    if (localization.Culture != null)
                    {
                        LocalizationDTO localizationDTO = new LocalizationDTO();

                        localizationDTO.Id = localization.Id;
                        localizationDTO.CultureId = localization.Culture.Id;
                        localizationDTO.Text = localization.Text;
                        localizationDTO.CultureName = localization.Culture.ShortName;
                        letterTypeDTO.Description.Add(localizationDTO);
                    }
                }
            }

            return letterTypeDTO;
        }

        private static LetterType MapLetterType(LetterTypeDTO letterTypeDTO, string cultureName)
        {
            if (letterTypeDTO == null)
                return null;
            LetterType letterType = new LetterType()
            {
                Id = letterTypeDTO.Id,
                Text = letterTypeDTO.LocalName,
                IsPopularization = letterTypeDTO.IsPopularization,
                TransactionCategories = TransactionCategoryMapper.Map(letterTypeDTO.TransactionCategories)
                
            };

            if (letterTypeDTO.Description != null)
            {
                letterType.LocalizationIdentifier.Localizations = new List<Localization>();

                foreach (LocalizationDTO localizationDTO in letterTypeDTO.Description)
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

            return letterType;
        }
    }
}