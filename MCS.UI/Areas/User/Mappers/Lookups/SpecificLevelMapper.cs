using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class SpecificLevelMapper
    {
        public static List<SpecificLevelVM> Map(IList<SpecificLevelDTO> specificLevelDTOs)
        {
            if (specificLevelDTOs == null || !specificLevelDTOs.Any())
            {
                return new List<SpecificLevelVM>();
            }
            List<SpecificLevelVM> specificLevelVMs = specificLevelDTOs
                .Select(specificLevelDTO => new SpecificLevelVM()
                {
                    Id = specificLevelDTO.Id,
                    Description = LocalizationMapper.Map(specificLevelDTO.Description),
                    LocalName = specificLevelDTO.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelDTO.TransactionCategories)
                }).ToList();
            return specificLevelVMs;
        }
        public static List<SpecificLevelDTO> Map(IList<SpecificLevelVM> specificLevelVMs)
        {
            if (specificLevelVMs == null || !specificLevelVMs.Any())
            {
                return new List<SpecificLevelDTO>();
            }
            List<SpecificLevelDTO> specificLevelDTOs = specificLevelVMs
                .Select(specificLevelVM => new SpecificLevelDTO()
                {
                    Id = specificLevelVM.Id,
                    Description = LocalizationMapper.Map(specificLevelVM.Description),
                    LocalName = specificLevelVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelVM.TransactionCategories)
                }).ToList();
            return specificLevelDTOs;
        }
        public static List<SpecificLevelAddDTO> Map(IList<SpecificLevelAddVM> specificLevelAddVMs)
        {
            if (specificLevelAddVMs == null || !specificLevelAddVMs.Any())
            {
                return new List<SpecificLevelAddDTO>();
            }
            List<SpecificLevelAddDTO> specificLevelAddDTOs = specificLevelAddVMs
                .Select(specificLevelAddVM => new SpecificLevelAddDTO()
                {
                    Description = LocalizationMapper.Map(specificLevelAddVM.Description),
                    //List = Map(specificLevelAddVM.List),
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelAddVM.TransactionCategories)
                }).ToList();
            return specificLevelAddDTOs;
        }
        public static List<SpecificLevelAddVM> Map(IList<SpecificLevelAddDTO> specificLevelAddDTOs)
        {
            if (specificLevelAddDTOs == null || !specificLevelAddDTOs.Any())
            {
                return new List<SpecificLevelAddVM>();
            }
            List<SpecificLevelAddVM> specificLevelAddVMs = specificLevelAddDTOs
                .Select(specificLevelAddDTO => new SpecificLevelAddVM()
                {
                    Description = LocalizationMapper.Map(specificLevelAddDTO.Description),
                    //List = Map(specificLevelAddDTO.List),
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelAddDTO.TransactionCategories)
                }).ToList();
            return specificLevelAddVMs;
        }
        //public static List<LetterListTypeVM> Map(IList<LetterListTypeDTO> letterListTypeDTOs)
        //{
        //    if (letterListTypeDTOs == null || !letterListTypeDTOs.Any())
        //    {
        //        return new List<LetterListTypeVM>();
        //    }
        //    List<LetterListTypeVM> letterListTypeVMs = letterListTypeDTOs
        //        .Select(letterListTypeDTO => new LetterListTypeVM()
        //        {
        //            Id = letterListTypeDTO.Id,
        //            IsSelected = letterListTypeDTO.IsSelected,
        //            Text = letterListTypeDTO.Text
        //        }).ToList();
        //    return letterListTypeVMs;
        //}
        //public static List<LetterListTypeDTO> Map(IList<LetterListTypeVM> letterListTypeVMs)
        //{
        //    if (letterListTypeVMs == null || !letterListTypeVMs.Any())
        //    {
        //        return new List<LetterListTypeDTO>();
        //    }
        //    List<LetterListTypeDTO> letterListTypeDTOs = letterListTypeVMs
        //        .Select(letterListTypeVM => new LetterListTypeDTO()
        //        {
        //            Id = letterListTypeVM.Id,
        //            IsSelected = letterListTypeVM.IsSelected,
        //            Text = letterListTypeVM.Text
        //        }).ToList();
        //    return letterListTypeDTOs;
        //}
        public static List<SpecificLevelEditDTO> Map(IList<SpecificLevelEditVM> specificLevelEditVMs)
        {
            if (specificLevelEditVMs == null || !specificLevelEditVMs.Any())
            {
                return new List<SpecificLevelEditDTO>();
            }
            List<SpecificLevelEditDTO> specificLevelEditDTOs = specificLevelEditVMs
                .Select(specificLevelEditVM => new SpecificLevelEditDTO()
                {
                    Id = specificLevelEditVM.Id,
                    Description = LocalizationMapper.Map(specificLevelEditVM.Description),
                    //List = Map(SpecificLevelEditVM.List),
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelEditVM.TransactionCategories)
                }).ToList();
            return specificLevelEditDTOs;
        }
        public static List<SpecificLevelEditVM> Map(IList<SpecificLevelEditDTO> specificLevelEditDTOs)
        {
            if (specificLevelEditDTOs == null || !specificLevelEditDTOs.Any())
            {
                return new List<SpecificLevelEditVM>();
            }
            List<SpecificLevelEditVM> specificLevelEditVMs = specificLevelEditDTOs
                .Select(specificLevelEditDTO => new SpecificLevelEditVM()
                {
                    Id = specificLevelEditDTO.Id,
                    Description = LocalizationMapper.Map(specificLevelEditDTO.Description), 
                    //List = Map(SpecificLevelEditDTO.List),
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelEditDTO.TransactionCategories)
                }).ToList();
            return specificLevelEditVMs;
        }
    }
}