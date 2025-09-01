using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class LetterTypeMapper
    {
        public static List<LetterTypeDTO> Map(IList<LetterTypeVM> letterTypeVMs)
        {
            if (letterTypeVMs == null || !letterTypeVMs.Any())
            { return null; }
            List<LetterTypeDTO> letterTypeDTOs = letterTypeVMs
                .Select(b => new LetterTypeDTO
                { 
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    IsPopularization = b.IsPopularization,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return letterTypeDTOs;
        }
        public static List<LetterTypeVM> Map(IList<LetterTypeDTO> letterTypeDTOs)
        {
            if (letterTypeDTOs == null || !letterTypeDTOs.Any())
            {
                return new List<LetterTypeVM>();
            }
            List<LetterTypeVM> letterTypeVMs = letterTypeDTOs
                .Select(b => new LetterTypeVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    IsPopularization = b.IsPopularization,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Notify = b.Notify,
                    WithExtraField = b.WithExtraField
                }).ToList();
            return letterTypeVMs;
        }
        public static List<LetterTypeEditDTO> Map(IList<LetterTypeEditVM> letterTypeVMs)
        {
            if (letterTypeVMs == null || !letterTypeVMs.Any())
            { return null; }
            List<LetterTypeEditDTO> letterTypeEditDTOs = letterTypeVMs
                .Select(b => new LetterTypeEditDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    IsPopularization = b.IsPopularization,
                    List = LetterListTypeMapper.Map(b.List),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return letterTypeEditDTOs;
        }
        public static List<LetterTypeEditVM> Map(IList<LetterTypeEditDTO> letterTypeEditDTOs)
        {
            if (letterTypeEditDTOs == null || !letterTypeEditDTOs.Any())
            { return null; }
            List<LetterTypeEditVM> letterTypeAddVMs = letterTypeEditDTOs
                .Select(b => new LetterTypeEditVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    IsPopularization = b.IsPopularization,
                    List = LetterListTypeMapper.Map(b.List),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return letterTypeAddVMs;
        }
        public static LetterTypeEditVM Map(LetterTypeEditDTO letterTypeEditDTO)
        {
            if (letterTypeEditDTO != null)
            {
                LetterTypeEditVM letterTypeEditVM = new LetterTypeEditVM()
                {
                    List = LetterListTypeMapper.Map(letterTypeEditDTO.List),
                    Description = LocalizationMapper.Map(letterTypeEditDTO.Description),
                    Id = letterTypeEditDTO.Id,
                    IsPopularization = letterTypeEditDTO.IsPopularization,
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeEditDTO.TransactionCategories)
                };
                return letterTypeEditVM;
            }
            return null;
        }
        public static LetterTypeEditDTO Map(LetterTypeEditVM letterTypeEditVM)
        {
            if (letterTypeEditVM != null)
            {
                LetterTypeEditDTO letterTypeEditDTO = new LetterTypeEditDTO()
                {
                    List = LetterListTypeMapper.Map(letterTypeEditVM.List),
                    Description = LocalizationMapper.Map(letterTypeEditVM.Description),
                    Id = letterTypeEditVM.Id,
                    IsPopularization = letterTypeEditVM.IsPopularization,
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeEditVM.TransactionCategories)
                };
                return letterTypeEditDTO;
            }
            return null;
        }
        public static List<LetterTypeAddDTO> Map(IList<LetterTypeAddVM> letterTypeAddVMs)
        {
            if (letterTypeAddVMs == null || !letterTypeAddVMs.Any())
            { return null; }
            List<LetterTypeAddDTO> letterTypeAddDTOs = letterTypeAddVMs
                .Select(b => new LetterTypeAddDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    IsPopularization = b.IsPopularization,
                    List = LetterListTypeMapper.Map(b.List),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return letterTypeAddDTOs;
        }
        public static LetterTypeAddDTO Map(LetterTypeAddVM letterTypeAddVM)
        {
            if (letterTypeAddVM != null)
            {
                LetterTypeAddDTO letterTypeAddDTO = new LetterTypeAddDTO()
                {
                    Description = LocalizationMapper.Map(letterTypeAddVM.Description),
                    IsPopularization = letterTypeAddVM.IsPopularization,
                    List = LetterListTypeMapper.Map(letterTypeAddVM.List),
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeAddVM.TransactionCategories)
                };
                return letterTypeAddDTO;
            }
            return null;
        }
        public static LetterTypeAddVM Map(LetterTypeAddDTO letterTypeAddDTO)
        {
            if (letterTypeAddDTO != null)
            {
                LetterTypeAddVM letterTypeAddVM = new LetterTypeAddVM()
                {
                    Description = LocalizationMapper.Map(letterTypeAddDTO.Description),
                    IsPopularization = letterTypeAddDTO.IsPopularization,
                    List = LetterListTypeMapper.Map(letterTypeAddDTO.List),
                    TransactionCategories = TransactionCategoryMapper.Map(letterTypeAddDTO.TransactionCategories)
                };
                return letterTypeAddVM;
            }
            return null;
        }
    }
}