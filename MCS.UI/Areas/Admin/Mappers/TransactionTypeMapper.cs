using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class TransactionTypeMapper
    {
        public static List<TransactionTypeDTO> Map(IList<TransactionTypeVM> transactionTypeVMs)
        {
            if (transactionTypeVMs == null || !transactionTypeVMs.Any())
            { return null; }
            List<TransactionTypeDTO> transactionTypeDTOs = transactionTypeVMs
                .Select(b => new TransactionTypeDTO
                {
                       
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)

                }).ToList();
            return transactionTypeDTOs;
        }
        public static List<TransactionTypeVM> Map(IList<TransactionTypeDTO> transactionTypeDTOs)
        {
            if (transactionTypeDTOs == null || !transactionTypeDTOs.Any())
            {
                return new List<TransactionTypeVM>();
            }
            List<TransactionTypeVM> transactionTypeVMs = transactionTypeDTOs
                .Select(b => new TransactionTypeVM
                {  
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)

                }).ToList();
            return transactionTypeVMs;
        }
        public static List<TransactionTypeAddDTO> Map(IList<TransactionTypeAddVM> transactionTypeAddVMs)
        {
            if (transactionTypeAddVMs == null || !transactionTypeAddVMs.Any())
            { return null; }
            List<TransactionTypeAddDTO> transactionTypeAddDTOs = transactionTypeAddVMs
                .Select(b => new TransactionTypeAddDTO
                {
                      
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Abbreviation = LocalizationMapper.Map(b.Abbreviation),
                    ColorId = b.ColorId,
                    PermissionId = b.PermissionId

                }).ToList();
            return transactionTypeAddDTOs;
        }
        public static TransactionTypeAddDTO Map(TransactionTypeAddVM transactionTypeAddVM)
        {
            if (transactionTypeAddVM != null)
            {
                TransactionTypeAddDTO transactionTypeAddDTO = new TransactionTypeAddDTO()
                {  
                    Description = LocalizationMapper.Map(transactionTypeAddVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeAddVM.TransactionCategories),
                    Abbreviation = LocalizationMapper.Map(transactionTypeAddVM.Abbreviation),
                    ColorId = transactionTypeAddVM.ColorId,
                    PermissionId = transactionTypeAddVM.PermissionId

                };
                return transactionTypeAddDTO;
            }
            return null;
        }
        public static TransactionTypeAddVM Map(TransactionTypeAddDTO transactionTypeAddDTO)
        {
            if (transactionTypeAddDTO != null)
            {
                TransactionTypeAddVM transactionTypeAddVM = new TransactionTypeAddVM()
                { 
                    Description = LocalizationMapper.Map(transactionTypeAddDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeAddDTO.TransactionCategories),
                    Abbreviation = LocalizationMapper.Map(transactionTypeAddDTO.Abbreviation),
                    ColorId = transactionTypeAddDTO.ColorId,
                    PermissionId = transactionTypeAddDTO.PermissionId

                };
                return transactionTypeAddVM;
            }
            return null;
        }
        public static List<TransactionTypeEditDTO> Map(IList<TransactionTypeEditVM> transactionTypeEditVMs)
        {
            if (transactionTypeEditVMs == null || !transactionTypeEditVMs.Any())
            { return null; }
            List<TransactionTypeEditDTO> transactionTypeEditDTOs = transactionTypeEditVMs
                .Select(b => new TransactionTypeEditDTO
                {
                    Id = b.Id,
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Abbreviation = LocalizationMapper.Map(b.Abbreviation),
                    ColorId = b.ColorId,
                    PermissionId = b.PermissionId

                }).ToList();
            return transactionTypeEditDTOs;
        }
        public static List<TransactionTypeEditVM> Map(IList<TransactionTypeEditDTO> transactionTypeEditDTOs)
        {
            if (transactionTypeEditDTOs == null || !transactionTypeEditDTOs.Any())
            { return null; }
            List<TransactionTypeEditVM> transactionTypeEditVMs = transactionTypeEditDTOs
                .Select(b => new TransactionTypeEditVM
                {
                    Id = b.Id,
                    Description = LocalizationMapper.Map(b.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    Abbreviation = LocalizationMapper.Map(b.Abbreviation),
                    ColorId = b.ColorId,
                    PermissionId = b.PermissionId

                }).ToList();
            return transactionTypeEditVMs;
        }
        public static TransactionTypeEditVM Map(TransactionTypeEditDTO transactionTypeEditDTO)
        {
            if (transactionTypeEditDTO != null)
            {
                TransactionTypeEditVM transactionTypeEditVM = new TransactionTypeEditVM()
                {
                    Id = transactionTypeEditDTO.Id,
                    Description = LocalizationMapper.Map(transactionTypeEditDTO.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeEditDTO.TransactionCategories),
                    Abbreviation = LocalizationMapper.Map(transactionTypeEditDTO.Abbreviation),
                    ColorId = transactionTypeEditDTO.ColorId,
                    PermissionId = transactionTypeEditDTO.PermissionId

                };
                return transactionTypeEditVM;
            }
            return null;
        }
        public static TransactionTypeEditDTO Map(TransactionTypeEditVM transactionTypeEditVM)
        {
            if (transactionTypeEditVM != null)
            {
                TransactionTypeEditDTO transactionTypeEditDTO = new TransactionTypeEditDTO()
                {
                    Id = transactionTypeEditVM.Id,
                    Description = LocalizationMapper.Map(transactionTypeEditVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(transactionTypeEditVM.TransactionCategories),
                    Abbreviation = LocalizationMapper.Map(transactionTypeEditVM.Abbreviation),
                    ColorId = transactionTypeEditVM.ColorId,
                    PermissionId = transactionTypeEditVM.PermissionId
                };
                return transactionTypeEditDTO;
            }
            return null;
        }
    }
}