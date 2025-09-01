using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class AttachmentTypeMapper
    {
        public static AttachmentType Map(AttachmentTypeAddDTO attachmentTypeAddDTO)
        {
            if (attachmentTypeAddDTO != null)
            {
                //TransactionCategories transactionCategories =
                //       TransactionCategoryMapper.Map(attachmentTypeAddDTO.TransactionCategories);

                AttachmentType attachmentType = new AttachmentType()
                {
                    TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                    LocalizationIdentifier = attachmentTypeAddDTO.Description !=null ? LocalizationIdentifierMapper.Map(attachmentTypeAddDTO.Description):null,
                    Archivable = attachmentTypeAddDTO.Archivable,
                    PrintBarcode = attachmentTypeAddDTO.PrintBarcode,
               
                };

                return attachmentType;
            }
            return null;
        }

        public static AttachmentType Map(AttachmentTypeEditDTO attachmentTypeEditDTO)
        {
            if (attachmentTypeEditDTO != null)
            {
               // TransactionCategories transactionCategories = TransactionCategoryMapper.Map(attachmentTypeEditDTO.TransactionCategories);

                AttachmentType attachmentType = new AttachmentType()
                {
                    Id = attachmentTypeEditDTO.Id,
                    TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                    LocalizationIdentifier = attachmentTypeEditDTO.Description !=null ? LocalizationIdentifierMapper.Map(attachmentTypeEditDTO.Description):null,
                    Archivable = attachmentTypeEditDTO.Archivable,
                    PrintBarcode = attachmentTypeEditDTO.PrintBarcode
                };

                return attachmentType;
            }
            return null;
        }

        public static AttachmentTypeEditDTO Map(AttachmentType attachmentType, string cultureName)
        {
            if (attachmentType != null)
            {
                AttachmentTypeEditDTO attachmentTypeEditDTO = new AttachmentTypeEditDTO()
                {
                    Id = attachmentType.Id,
                    Description = attachmentType.LocalizationIdentifier.Localizations !=null ? LocalizationIdentifierMapper.Map(attachmentType.LocalizationIdentifier.Localizations):null,
                    TransactionCategories = TransactionCategoryMapper.Map(attachmentType.TransactionCategories, cultureName),
                    Archivable = attachmentType.Archivable,
                    PrintBarcode = attachmentType.PrintBarcode,
                    IsActive = attachmentType.IsActive,
                    IsLocked = attachmentType.IsLocked,
                    LockedBy = attachmentType.LockedBy
                };

                return attachmentTypeEditDTO;
            }
            return null;
        }

        public static List<AttachmentTypeDTO> Map(IList<AttachmentType> attachmentTypes, string cultureName)
        {
            if (attachmentTypes == null || !attachmentTypes.Any())
            {
                return null;
            }
            List<AttachmentTypeDTO> attachmentTypeDTOs = attachmentTypes
            .Select(attachmentTypeDTO => new AttachmentTypeDTO()
            {
                Id = attachmentTypeDTO.Id,
                Archivable = attachmentTypeDTO.Archivable,
                TransactionCategories = TransactionCategoryMapper.Map(attachmentTypeDTO.TransactionCategories, cultureName),
                LocalName = attachmentTypeDTO.Text,
                Description = attachmentTypeDTO.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(attachmentTypeDTO.LocalizationIdentifier.Localizations) : null,
                IsActive = attachmentTypeDTO.IsActive,
                IsLocked = attachmentTypeDTO.IsLocked,
                LockedBy = attachmentTypeDTO.LockedBy
            }).ToList();
            return attachmentTypeDTOs;


        }

        public static List<AttachmentType> Map(IList<AttachmentTypeDTO> attachmentTypesDTO, string cultureName)
        {
            if (attachmentTypesDTO == null || !attachmentTypesDTO.Any())
            {
                return null;
            }
            List<AttachmentType> attachmentTypes = attachmentTypesDTO
            .Select(attachmentType => new AttachmentType()
            {

                Id = attachmentType.Id,
                Archivable = attachmentType.Archivable,
                Text = attachmentType.LocalName,
                TransactionCategories = TransactionCategoryMapper.Map(attachmentType.TransactionCategories),
                LocalizationIdentifier = attachmentType.Description !=null ? LocalizationIdentifierMapper.Map(attachmentType.Description):null

            }).ToList();
            return attachmentTypes;


        }


    }
}