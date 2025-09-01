using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class TransactionTypeMapper
    {
        public static List<TransactionTypeVM> Map(IList<TransactionTypeDTO> transactionTypeDTOs)
        {
            if (transactionTypeDTOs == null || !transactionTypeDTOs.Any())
            {
                return new List<TransactionTypeVM>();
            }
            List<TransactionTypeVM> transactionTypeVMs = transactionTypeDTOs
                .Select(transactionTypeDTO => new TransactionTypeVM()
                { 
                    Id = transactionTypeDTO.Id,
                    Description = LocalizationMapper.Map(transactionTypeDTO.Description),
                    LocalName = transactionTypeDTO.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeDTO.TransactionCategories)
                }).ToList();
            return transactionTypeVMs;
        }
        public static List<TransactionTypeDTO> Map(IList<TransactionTypeVM> transactionTypeVMs)
        {
            if (transactionTypeVMs == null || !transactionTypeVMs.Any())
            {
                return new List<TransactionTypeDTO>();
            }
            List<TransactionTypeDTO> transactionTypeDTOs = transactionTypeVMs
                .Select(transactionTypeVM => new TransactionTypeDTO()
                {
                    Id = transactionTypeVM.Id,
                    Description = LocalizationMapper.Map(transactionTypeVM.Description),
                    LocalName = transactionTypeVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeVM.TransactionCategories)
                }).ToList();
            return transactionTypeDTOs;
        }
        public static List<TransactionTypeAddVM> Map(IList<TransactionTypeAddDTO> transactionTypeAddDTOs)
        {
            if (transactionTypeAddDTOs == null || !transactionTypeAddDTOs.Any())
            {
                return new List<TransactionTypeAddVM>();
            }
            List<TransactionTypeAddVM> transactionTypeAddVMs = transactionTypeAddDTOs
                .Select(transactionTypeAddDTO => new TransactionTypeAddVM()
                { 
                    PermissionId = transactionTypeAddDTO.PermissionId,
                    Abbreviation = LocalizationMapper.Map(transactionTypeAddDTO.Abbreviation),
                    ColorId = transactionTypeAddDTO.ColorId,
                    Description = LocalizationMapper.Map(transactionTypeAddDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeAddDTO.TransactionCategories)
                }).ToList();
            return transactionTypeAddVMs;
        }
        public static List<TransactionTypeAddDTO> Map(IList<TransactionTypeAddVM> transactionTypeAddVMs)
        {
            if (transactionTypeAddVMs == null || !transactionTypeAddVMs.Any())
            {
                return new List<TransactionTypeAddDTO>();
            }
            List<TransactionTypeAddDTO> transactionTypeAddDTOs = transactionTypeAddVMs
                .Select(transactionTypeAddVM => new TransactionTypeAddDTO()
                {
                    PermissionId = transactionTypeAddVM.PermissionId,
                    Abbreviation = LocalizationMapper.Map(transactionTypeAddVM.Abbreviation),
                    ColorId = transactionTypeAddVM.ColorId,
                    Description = LocalizationMapper.Map(transactionTypeAddVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeAddVM.TransactionCategories)
                }).ToList();
            return transactionTypeAddDTOs;
        }
        public static List<TransactionTypeEditDTO> Map(IList<TransactionTypeEditVM> transactionTypeEditVMs)
        {
            if (transactionTypeEditVMs == null || !transactionTypeEditVMs.Any())
            {
                return new List<TransactionTypeEditDTO>();
            }
            List<TransactionTypeEditDTO> transactionTypeEditDTOs = transactionTypeEditVMs
                .Select(transactionTypeEditVM => new TransactionTypeEditDTO()
                {
                    Id = transactionTypeEditVM.Id,
                    PermissionId = transactionTypeEditVM.PermissionId,
                    Abbreviation = LocalizationMapper.Map(transactionTypeEditVM.Abbreviation),
                    ColorId = transactionTypeEditVM.ColorId,
                    Description = LocalizationMapper.Map(transactionTypeEditVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeEditVM.TransactionCategories)
                }).ToList();
            return transactionTypeEditDTOs;
        }
        public static List<TransactionTypeEditVM> Map(IList<TransactionTypeEditDTO> transactionTypeEditDTOs)
        {
            if (transactionTypeEditDTOs == null || !transactionTypeEditDTOs.Any())
            {
                return new List<TransactionTypeEditVM>();
            }
            List<TransactionTypeEditVM> transactionTypeEditVMs = transactionTypeEditDTOs
                .Select(transactionTypeEditDTO => new TransactionTypeEditVM()
                { 
                    Id = transactionTypeEditDTO.Id,
                    PermissionId = transactionTypeEditDTO.PermissionId,
                    Abbreviation = LocalizationMapper.Map(transactionTypeEditDTO.Abbreviation),
                    ColorId = transactionTypeEditDTO.ColorId,
                    Description = LocalizationMapper.Map(transactionTypeEditDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeEditDTO.TransactionCategories)
                }).ToList();
            return transactionTypeEditVMs;
        }

    }
}