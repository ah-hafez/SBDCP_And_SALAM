using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class LetterTypeMapper
    {
        public static List<LetterTypeVM> Map(IList<LetterTypeDTO> letterTypeDTOs)
        {
            if (letterTypeDTOs == null || !letterTypeDTOs.Any())
            {
                return new List<LetterTypeVM>();
            }
            List<LetterTypeVM> letterTypeVMs = letterTypeDTOs
                .Select(letterTypeDTO => new LetterTypeVM()
                {
                    Id = letterTypeDTO.Id,
                    Description = LocalizationMapper.Map(letterTypeDTO.Description),
                    IsPopularization = letterTypeDTO.IsPopularization,
                    LocalName = letterTypeDTO.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeDTO.TransactionCategories)
                }).ToList();
            return letterTypeVMs;
        }
        public static List<LetterTypeDTO> Map(IList<LetterTypeVM> letterTypeVMs)
        {
            if (letterTypeVMs == null || !letterTypeVMs.Any())
            {
                return new List<LetterTypeDTO>();
            }
            List<LetterTypeDTO> letterTypeDTOs = letterTypeVMs
                .Select(letterTypeVM => new LetterTypeDTO()
                {
                    Id = letterTypeVM.Id,
                    Description = LocalizationMapper.Map(letterTypeVM.Description),
                    IsPopularization = letterTypeVM.IsPopularization,
                    LocalName = letterTypeVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeVM.TransactionCategories)
                }).ToList();
            return letterTypeDTOs;
        }
        public static List<LetterTypeAddDTO> Map(IList<LetterTypeAddVM> letterTypeAddVMs)
        {
            if (letterTypeAddVMs == null || !letterTypeAddVMs.Any())
            {
                return new List<LetterTypeAddDTO>();
            }
            List<LetterTypeAddDTO> letterTypeAddDTOs = letterTypeAddVMs
                .Select(letterTypeAddVM => new LetterTypeAddDTO()
                {
                    Description = LocalizationMapper.Map(letterTypeAddVM.Description),
                    IsPopularization = letterTypeAddVM.IsPopularization,
                    List = Map(letterTypeAddVM.List),
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeAddVM.TransactionCategories)
                }).ToList();
            return letterTypeAddDTOs;
        }
        public static List<LetterTypeAddVM> Map(IList<LetterTypeAddDTO> letterTypeAddDTOs)
        {
            if (letterTypeAddDTOs == null || !letterTypeAddDTOs.Any())
            {
                return new List<LetterTypeAddVM>();
            }
            List<LetterTypeAddVM> letterTypeAddVMs = letterTypeAddDTOs
                .Select(letterTypeAddDTO => new LetterTypeAddVM()
                {
                    Description = LocalizationMapper.Map(letterTypeAddDTO.Description),
                    IsPopularization = letterTypeAddDTO.IsPopularization,
                    List = Map(letterTypeAddDTO.List),
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeAddDTO.TransactionCategories)
                }).ToList();
            return letterTypeAddVMs;
        }
        public static List<LetterListTypeVM> Map(IList<LetterListTypeDTO> letterListTypeDTOs)
        {
            if (letterListTypeDTOs == null || !letterListTypeDTOs.Any())
            {
                return new List<LetterListTypeVM>();
            }
            List<LetterListTypeVM> letterListTypeVMs = letterListTypeDTOs
                .Select(letterListTypeDTO => new LetterListTypeVM()
                {
                    Id = letterListTypeDTO.Id,
                    IsSelected = letterListTypeDTO.IsSelected,
                    Text = letterListTypeDTO.Text
                }).ToList();
            return letterListTypeVMs;
        }
        public static List<LetterListTypeDTO> Map(IList<LetterListTypeVM> letterListTypeVMs)
        {
            if (letterListTypeVMs == null || !letterListTypeVMs.Any())
            {
                return new List<LetterListTypeDTO>();
            }
            List<LetterListTypeDTO> letterListTypeDTOs = letterListTypeVMs
                .Select(letterListTypeVM => new LetterListTypeDTO()
                {
                    Id = letterListTypeVM.Id,
                    IsSelected = letterListTypeVM.IsSelected,
                    Text = letterListTypeVM.Text
                }).ToList();
            return letterListTypeDTOs;
        }
        public static List<LetterTypeEditDTO> Map(IList<LetterTypeEditVM> letterTypeEditVMs)
        {
            if (letterTypeEditVMs == null || !letterTypeEditVMs.Any())
            {
                return new List<LetterTypeEditDTO>();
            }
            List<LetterTypeEditDTO> letterTypeEditDTOs = letterTypeEditVMs
                .Select(letterTypeEditVM => new LetterTypeEditDTO()
                {
                    Id = letterTypeEditVM.Id,
                    Description = LocalizationMapper.Map(letterTypeEditVM.Description),
                    IsPopularization = letterTypeEditVM.IsPopularization,
                    List = Map(letterTypeEditVM.List),
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeEditVM.TransactionCategories)
                }).ToList();
            return letterTypeEditDTOs;
        }
        public static List<LetterTypeEditVM> Map(IList<LetterTypeEditDTO> letterTypeEditDTOs)
        {
            if (letterTypeEditDTOs == null || !letterTypeEditDTOs.Any())
            {
                return new List<LetterTypeEditVM>();
            }
            List<LetterTypeEditVM> letterTypeEditVMs = letterTypeEditDTOs
                .Select(letterTypeEditDTO => new LetterTypeEditVM()
                {
                    Id = letterTypeEditDTO.Id,
                    Description = LocalizationMapper.Map(letterTypeEditDTO.Description),
                    IsPopularization = letterTypeEditDTO.IsPopularization,
                    List = Map(letterTypeEditDTO.List),
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeEditDTO.TransactionCategories)
                }).ToList();
            return letterTypeEditVMs;
        }
    }
}