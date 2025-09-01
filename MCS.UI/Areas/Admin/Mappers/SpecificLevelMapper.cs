using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class SpecificLevelMapper
    {
        public static List<SpecificLevelDTO> Map(IList<SpecificLevelVM> specificLevelVMs)
        {
            if (specificLevelVMs == null || !specificLevelVMs.Any())
            { return null; }
            List<SpecificLevelDTO> specificLevelDTOs = specificLevelVMs
                .Select(b => new SpecificLevelDTO
                { 
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return specificLevelDTOs;
        }
        public static List<SpecificLevelVM> Map(IList<SpecificLevelDTO> specificLevelDTOs)
        {
            if (specificLevelDTOs == null || !specificLevelDTOs.Any())
            {
                return new List<SpecificLevelVM>();
            }
            List<SpecificLevelVM> specificLevelVMs = specificLevelDTOs
                .Select(b => new SpecificLevelVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                }).ToList();
            return specificLevelVMs;
        }
        public static List<SpecificLevelEditDTO> Map(IList<SpecificLevelEditVM> specificLevelEditVMs)
        {
            if (specificLevelEditVMs == null || !specificLevelEditVMs.Any())
            { return null; }
            List<SpecificLevelEditDTO> specificLevelEditDTOs = specificLevelEditVMs
                .Select(b => new SpecificLevelEditDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    List = SpecificLevelListMapper.Map(b.List),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return specificLevelEditDTOs;
        }
        public static List<SpecificLevelEditVM> Map(IList<SpecificLevelEditDTO> specificLevelEditDTOs)
        {
            if (specificLevelEditDTOs == null || !specificLevelEditDTOs.Any())
            { return null; }
            List<SpecificLevelEditVM> specificLevelEditVMs = specificLevelEditDTOs
                .Select(b => new SpecificLevelEditVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    List = SpecificLevelListMapper.Map(b.List),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return specificLevelEditVMs;
        }
        public static SpecificLevelEditVM Map(SpecificLevelEditDTO specificLevelEditDTO)
        {
            if (specificLevelEditDTO != null)
            {
                SpecificLevelEditVM specificLevelEditVM = new SpecificLevelEditVM()
                {
                    List = SpecificLevelListMapper.Map(specificLevelEditDTO.List),
                    Description = LocalizationMapper.Map(specificLevelEditDTO.Description),
                    Id = specificLevelEditDTO.Id,
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelEditDTO.TransactionCategories)
                };
                return specificLevelEditVM;
            }
            return null;
        }
        public static SpecificLevelEditDTO Map(SpecificLevelEditVM specificLevelEditVM)
        {
            if (specificLevelEditVM != null)
            {
                SpecificLevelEditDTO specificLevelEditDTO = new SpecificLevelEditDTO()
                {
                    List = SpecificLevelListMapper.Map(specificLevelEditVM.List),
                    Description = LocalizationMapper.Map(specificLevelEditVM.Description),
                    Id = specificLevelEditVM.Id,
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelEditVM.TransactionCategories)
                };
                return specificLevelEditDTO;
            }
            return null;
        }
        public static List<SpecificLevelAddDTO> Map(IList<SpecificLevelAddVM> specificLevelAddVMs)
        {
            if (specificLevelAddVMs == null || !specificLevelAddVMs.Any())
            { return null; }
            List<SpecificLevelAddDTO> specificLevelAddDTOs = specificLevelAddVMs
                .Select(b => new SpecificLevelAddDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    List = SpecificLevelListMapper.Map(b.List),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return specificLevelAddDTOs;
        }
        public static SpecificLevelAddDTO Map(SpecificLevelAddVM specificLevelAddVM)
        {
            if (specificLevelAddVM != null)
            {
                SpecificLevelAddDTO specificLevelAddDTO = new SpecificLevelAddDTO()
                {
                    Description = LocalizationMapper.Map(specificLevelAddVM.Description),
                    List = SpecificLevelListMapper.Map(specificLevelAddVM.List),
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelAddVM.TransactionCategories)
                };
                return specificLevelAddDTO;
            }
            return null;
        }
        public static SpecificLevelAddVM Map(SpecificLevelAddDTO specificLevelAddDTO)
        {
            if (specificLevelAddDTO != null)
            {
                SpecificLevelAddVM specificLevelAddVM = new SpecificLevelAddVM()
                {
                    Description = LocalizationMapper.Map(specificLevelAddDTO.Description),
                    List = SpecificLevelListMapper.Map(specificLevelAddDTO.List),
                    TransactionCategories = TransactionCategoryMapper.Map(specificLevelAddDTO.TransactionCategories)
                };
                return specificLevelAddVM;
            }
            return null;
        }
    }
}