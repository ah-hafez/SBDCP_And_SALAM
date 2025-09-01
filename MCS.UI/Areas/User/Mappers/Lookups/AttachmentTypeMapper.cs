using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class AttachmentTypeMapper
    {
        public static List<AttachmentTypeVM> Map(IList<AttachmentTypeDTO> attachmentTypeDTOs)
        {
            if (attachmentTypeDTOs == null || !attachmentTypeDTOs.Any())
            {
                return new List<AttachmentTypeVM>();
            }
            List<AttachmentTypeVM> attachmentTypeVMs = attachmentTypeDTOs
                .Select(attachmentTypeDTO => new AttachmentTypeVM()
                { 
                    Id = attachmentTypeDTO.Id,
                    Archivable = attachmentTypeDTO.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeDTO.Description),
                    LocalName = attachmentTypeDTO.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeDTO.TransactionCategories)
                }).ToList();

            return attachmentTypeVMs;
        }
        public static List<AttachmentTypeDTO> Map(IList<AttachmentTypeVM> attachmentTypeVMs)
        {
            if (attachmentTypeVMs == null || !attachmentTypeVMs.Any())
            {
                return new List<AttachmentTypeDTO>();
            }
            List<AttachmentTypeDTO> attachmentTypeDTOs = attachmentTypeVMs
                .Select(attachmentTypeVM => new AttachmentTypeDTO()
                {
                    Id = attachmentTypeVM.Id,
                    Archivable = attachmentTypeVM.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeVM.Description),
                    LocalName = attachmentTypeVM.LocalName,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeVM.TransactionCategories)
                }).ToList();

            return attachmentTypeDTOs;
        }
        public static List<AttachmentTypeAddVM> Map(IList<AttachmentTypeAddDTO> attachmentTypeAddDTOs)
        {
            if (attachmentTypeAddDTOs == null || !attachmentTypeAddDTOs.Any())
            {
                return new List<AttachmentTypeAddVM>();
            }
            List<AttachmentTypeAddVM> attachmentTypeAddVMs = attachmentTypeAddDTOs
                .Select(attachmentTypeVM => new AttachmentTypeAddVM()
                {
                    Archivable = attachmentTypeVM.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeVM.TransactionCategories),
                    PrintBarcode = attachmentTypeVM.PrintBarcode
                }).ToList();

            return attachmentTypeAddVMs;
        }
        public static List<AttachmentTypeAddDTO> Map(IList<AttachmentTypeAddVM> attachmentTypeAddVMs)
        {
            if (attachmentTypeAddVMs == null || !attachmentTypeAddVMs.Any())
            {
                return new List<AttachmentTypeAddDTO>();
            }
            List<AttachmentTypeAddDTO> attachmentTypeAddDTOs = attachmentTypeAddVMs
                .Select(attachmentTypeAddVM => new AttachmentTypeAddDTO()
                {
                    Archivable = attachmentTypeAddVM.Archivable,
                    Description = LocalizationMapper.Map(attachmentTypeAddVM.Description),
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeAddVM.TransactionCategories),
                    PrintBarcode = attachmentTypeAddVM.PrintBarcode
                }).ToList();

            return attachmentTypeAddDTOs;

        }
        public static List<AttachmentTypeEditVM> Map(IList<AttachmentTypeEditDTO> attachmentTypeEditDTOs)
        {
            if (attachmentTypeEditDTOs == null || !attachmentTypeEditDTOs.Any())
            {
                return new List<AttachmentTypeEditVM>();
            }
            List<AttachmentTypeEditVM> attachmentTypeEditVMs = attachmentTypeEditDTOs
                .Select(attachmentTypeEditDTO => new AttachmentTypeEditVM()
                {
                    Id = attachmentTypeEditDTO.Id,
                    Description = LocalizationMapper.Map(attachmentTypeEditDTO.Description),
                    PrintBarcode = attachmentTypeEditDTO.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeEditDTO.TransactionCategories),
                    Archivable = attachmentTypeEditDTO.Archivable
                }).ToList();

            return attachmentTypeEditVMs;
        }
        public static List<AttachmentTypeEditDTO> Map(IList<AttachmentTypeEditVM> attachmentTypeEditVMs)
        {
            if (attachmentTypeEditVMs == null || !attachmentTypeEditVMs.Any())
            {
                return new List<AttachmentTypeEditDTO>();
            }
            List<AttachmentTypeEditDTO> attachmentTypeEditDTOs = attachmentTypeEditVMs
                .Select(attachmentTypeEditVM => new AttachmentTypeEditDTO()
                { 
                    Id = attachmentTypeEditVM.Id,
                    Description = LocalizationMapper.Map(attachmentTypeEditVM.Description),
                    PrintBarcode = attachmentTypeEditVM.PrintBarcode,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeEditVM.TransactionCategories),
                    Archivable = attachmentTypeEditVM.Archivable
                }).ToList();

            return attachmentTypeEditDTOs;
        }

    }
}