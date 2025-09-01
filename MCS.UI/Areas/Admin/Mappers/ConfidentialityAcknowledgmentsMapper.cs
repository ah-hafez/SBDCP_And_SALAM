using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class ConfidentialityAcknowledgmentsMapper
    {
        public static List<ConfidentialityAcknowledgmentsDTO> Map(IList<ConfidentialityAcknowledgmentsVM> ConfidentialityAcknowledgmentsVMs)
        {
            if (ConfidentialityAcknowledgmentsVMs == null || !ConfidentialityAcknowledgmentsVMs.Any())
            {
                return null;
            }
            List<ConfidentialityAcknowledgmentsDTO> ConfidentialityAcknowledgmentsDTOs = ConfidentialityAcknowledgmentsVMs
                .Select(b => new ConfidentialityAcknowledgmentsDTO
                {
                    IsMandatary = b.IsMandatary,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return ConfidentialityAcknowledgmentsDTOs;
        }
        public static List<ConfidentialityAcknowledgmentsVM> Map(IList<ConfidentialityAcknowledgmentsDTO> ConfidentialityAcknowledgmentsDTOs)
        {
            if (ConfidentialityAcknowledgmentsDTOs == null || !ConfidentialityAcknowledgmentsDTOs.Any())
            {
                return new List<ConfidentialityAcknowledgmentsVM>();
            }
            List<ConfidentialityAcknowledgmentsVM> ConfidentialityAcknowledgmentsVMs = ConfidentialityAcknowledgmentsDTOs
                .Select(b => new ConfidentialityAcknowledgmentsVM
                {
                    IsMandatary = b.IsMandatary,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return ConfidentialityAcknowledgmentsVMs;
        }
        public static ConfidentialityAcknowledgmentsAddDTO Map(ConfidentialityAcknowledgmentsAddVM ConfidentialityAcknowledgmentsAddVM)
        {
            if (ConfidentialityAcknowledgmentsAddVM != null)
            {
                ConfidentialityAcknowledgmentsAddDTO ConfidentialityAcknowledgmentsAddDTO = new ConfidentialityAcknowledgmentsAddDTO()
                {
                    IsMandatary = ConfidentialityAcknowledgmentsAddVM.IsMandatary,
                    Description = LocalizationMapper.Map(ConfidentialityAcknowledgmentsAddVM.Description), 
                    TransactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgmentsAddVM.TransactionCategories)
                };
                return ConfidentialityAcknowledgmentsAddDTO;
            }
            return null;
        }
        public static ConfidentialityAcknowledgmentsAddVM Map(ConfidentialityAcknowledgmentsAddDTO ConfidentialityAcknowledgmentsAddDTO)
        {
            if (ConfidentialityAcknowledgmentsAddDTO != null)
            {
                ConfidentialityAcknowledgmentsAddVM ConfidentialityAcknowledgmentsAddVM = new ConfidentialityAcknowledgmentsAddVM()
                {
                    IsMandatary = ConfidentialityAcknowledgmentsAddDTO.IsMandatary,
                    Description = LocalizationMapper.Map(ConfidentialityAcknowledgmentsAddDTO.Description), 
                    TransactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgmentsAddDTO.TransactionCategories)
                };
                return ConfidentialityAcknowledgmentsAddVM;
            }
            return null;
        }
        public static ConfidentialityAcknowledgmentsEditDTO Map(ConfidentialityAcknowledgmentsEditVM ConfidentialityAcknowledgmentsEditVM)
        {
            if (ConfidentialityAcknowledgmentsEditVM != null)
            {
                ConfidentialityAcknowledgmentsEditDTO ConfidentialityAcknowledgmentsEditDTO = new ConfidentialityAcknowledgmentsEditDTO()
                {
                    Id = ConfidentialityAcknowledgmentsEditVM.Id,
                    IsMandatary = ConfidentialityAcknowledgmentsEditVM.IsMandatary,
                    Description = LocalizationMapper.Map(ConfidentialityAcknowledgmentsEditVM.Description), 
                    TransactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgmentsEditVM.TransactionCategories)
                };
                return ConfidentialityAcknowledgmentsEditDTO;
            }
            return null;
        }
        public static ConfidentialityAcknowledgmentsEditVM Map(ConfidentialityAcknowledgmentsEditDTO ConfidentialityAcknowledgmentsEditDTO)
        {
            if (ConfidentialityAcknowledgmentsEditDTO != null)
            {
                ConfidentialityAcknowledgmentsEditVM ConfidentialityAcknowledgmentsEditVM = new ConfidentialityAcknowledgmentsEditVM()
                {
                    Id = ConfidentialityAcknowledgmentsEditDTO.Id,
                    IsMandatary = ConfidentialityAcknowledgmentsEditDTO.IsMandatary,
                    Description = LocalizationMapper.Map(ConfidentialityAcknowledgmentsEditDTO.Description), 
                    TransactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgmentsEditDTO.TransactionCategories),
                    IsActive = ConfidentialityAcknowledgmentsEditDTO.IsActive,
                    IsLocked = ConfidentialityAcknowledgmentsEditDTO.IsLocked,
                    LockedBy = ConfidentialityAcknowledgmentsEditDTO.LockedBy
                };
                return ConfidentialityAcknowledgmentsEditVM;
            }
            return null;
        }
        public static List<ConfidentialityAcknowledgmentsAddDTO> Map(IList<ConfidentialityAcknowledgmentsAddVM> ConfidentialityAcknowledgmentsAddVMs)
        {
            if (ConfidentialityAcknowledgmentsAddVMs == null || !ConfidentialityAcknowledgmentsAddVMs.Any())
            {
                return null;
            }
            List<ConfidentialityAcknowledgmentsAddDTO> ConfidentialityAcknowledgmentsAddDTOs = ConfidentialityAcknowledgmentsAddVMs
                .Select(b => new ConfidentialityAcknowledgmentsAddDTO
                {
                    IsMandatary = b.IsMandatary,
                    Description = LocalizationMapper.Map(b.Description), 
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return ConfidentialityAcknowledgmentsAddDTOs;
        }
        public static List<ConfidentialityAcknowledgmentsEditDTO> Map(IList<ConfidentialityAcknowledgmentsEditVM> ConfidentialityAcknowledgmentsEditVMs)
        {
            if (ConfidentialityAcknowledgmentsEditVMs == null || !ConfidentialityAcknowledgmentsEditVMs.Any())
            {
                return null;
            }
            List<ConfidentialityAcknowledgmentsEditDTO> ConfidentialityAcknowledgmentsEditDTOs = ConfidentialityAcknowledgmentsEditVMs
                .Select(b => new ConfidentialityAcknowledgmentsEditDTO
                {
                    IsMandatary = b.IsMandatary,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id, 
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return ConfidentialityAcknowledgmentsEditDTOs;
        }
        public static List<ConfidentialityAcknowledgmentsEditVM> Map(IList<ConfidentialityAcknowledgmentsEditDTO> ConfidentialityAcknowledgmentsEditDTOs)
        {
            if (ConfidentialityAcknowledgmentsEditDTOs == null || !ConfidentialityAcknowledgmentsEditDTOs.Any())
            {
                return null;
            }
            List<ConfidentialityAcknowledgmentsEditVM> ConfidentialityAcknowledgmentsEditVMs = ConfidentialityAcknowledgmentsEditDTOs
                .Select(b => new ConfidentialityAcknowledgmentsEditVM
                {
                    IsMandatary = b.IsMandatary,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id, 
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return ConfidentialityAcknowledgmentsEditVMs;
        }
    }
}