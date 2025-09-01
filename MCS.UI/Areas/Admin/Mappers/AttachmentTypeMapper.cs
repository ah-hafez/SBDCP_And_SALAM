using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class AttachmentTypeMapper
    {
        public static List<AttachmentTypeDTO> Map(IList<AttachmentTypeVM> attachmentTypeVMs)
        {
            if (attachmentTypeVMs == null || !attachmentTypeVMs.Any())
            {
                return null;
            }
            List<AttachmentTypeDTO> attachmentTypeDTOs = attachmentTypeVMs
                .Select(b => new AttachmentTypeDTO
                {
                    Archivable = b.Archivable,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return attachmentTypeDTOs;
        }
        public static List<AttachmentTypeVM> Map(IList<AttachmentTypeDTO> attachmentTypeDTOs)
        {
            if (attachmentTypeDTOs == null || !attachmentTypeDTOs.Any())
            {
                return new List<AttachmentTypeVM>();
            }
            List<AttachmentTypeVM> attachmentTypeVMs = attachmentTypeDTOs
                .Select(b => new AttachmentTypeVM
                {
                    Archivable = b.Archivable,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return attachmentTypeVMs;
        }
        public static AttachmentTypeAddDTO Map(AttachmentTypeAddVM attachmentTypeAddVM)
        {
            if (attachmentTypeAddVM != null)
            {
                AttachmentTypeAddDTO attachmentTypeAddDTO = new AttachmentTypeAddDTO()
                {
                    Archivable = attachmentTypeAddVM.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeAddVM.Description),
                    PrintBarcode = attachmentTypeAddVM.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeAddVM.TransactionCategories)
                };
                return attachmentTypeAddDTO;
            }
            return null;
        }
        public static AttachmentTypeAddVM Map(AttachmentTypeAddDTO attachmentTypeAddDTO)
        {
            if (attachmentTypeAddDTO != null)
            {
                AttachmentTypeAddVM attachmentTypeAddVM = new AttachmentTypeAddVM()
                {
                    Archivable = attachmentTypeAddDTO.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeAddDTO.Description),
                    PrintBarcode = attachmentTypeAddDTO.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeAddDTO.TransactionCategories)
                };
                return attachmentTypeAddVM;
            }
            return null;
        }
        public static AttachmentTypeEditDTO Map(AttachmentTypeEditVM attachmentTypeEditVM)
        {
            if (attachmentTypeEditVM != null)
            {
                AttachmentTypeEditDTO attachmentTypeEditDTO = new AttachmentTypeEditDTO()
                {
                    Id = attachmentTypeEditVM.Id,
                    Archivable = attachmentTypeEditVM.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeEditVM.Description),
                    PrintBarcode = attachmentTypeEditVM.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeEditVM.TransactionCategories)
                };
                return attachmentTypeEditDTO;
            }
            return null;
        }
        public static AttachmentTypeEditVM Map(AttachmentTypeEditDTO attachmentTypeEditDTO)
        {
            if (attachmentTypeEditDTO != null)
            {
                AttachmentTypeEditVM attachmentTypeEditVM = new AttachmentTypeEditVM()
                {
                    Id = attachmentTypeEditDTO.Id,
                    Archivable = attachmentTypeEditDTO.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeEditDTO.Description),
                    PrintBarcode = attachmentTypeEditDTO.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeEditDTO.TransactionCategories),
                    IsActive = attachmentTypeEditDTO.IsActive,
                    IsLocked = attachmentTypeEditDTO.IsLocked,
                    LockedBy = attachmentTypeEditDTO.LockedBy
                };
                return attachmentTypeEditVM;
            }
            return null;
        }
        public static List<AttachmentTypeAddDTO> Map(IList<AttachmentTypeAddVM> attachmentTypeAddVMs)
        {
            if (attachmentTypeAddVMs == null || !attachmentTypeAddVMs.Any())
            {
                return null;
            }
            List<AttachmentTypeAddDTO> attachmentTypeAddDTOs = attachmentTypeAddVMs
                .Select(b => new AttachmentTypeAddDTO
                {
                    Archivable = b.Archivable,
                    Description = LocalizationMapper.Map(b.Description),
                    PrintBarcode = b.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return attachmentTypeAddDTOs;
        }
        public static List<AttachmentTypeEditDTO> Map(IList<AttachmentTypeEditVM> attachmentTypeEditVMs)
        {
            if (attachmentTypeEditVMs == null || !attachmentTypeEditVMs.Any())
            {
                return null;
            }
            List<AttachmentTypeEditDTO> attachmentTypeEditDTOs = attachmentTypeEditVMs
                .Select(b => new AttachmentTypeEditDTO
                {
                    Archivable = b.Archivable,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    PrintBarcode = b.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories)
                }).ToList();
            return attachmentTypeEditDTOs;
        }
        public static List<AttachmentTypeEditVM> Map(IList<AttachmentTypeEditDTO> attachmentTypeEditDTOs)
        {
            if (attachmentTypeEditDTOs == null || !attachmentTypeEditDTOs.Any())
            {
                return null;
            }
            List<AttachmentTypeEditVM> attachmentTypeEditVMs = attachmentTypeEditDTOs
                .Select(b => new AttachmentTypeEditVM
                {
                    Archivable = b.Archivable,
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    PrintBarcode = b.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(b.TransactionCategories),
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return attachmentTypeEditVMs;
        }
    }
}