using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class ConfidentialityAcknowledgmentsMapper
    {
        public static ConfidentialityAcknowledgment Map(ConfidentialityAcknowledgmentsAddDTO ConfidentialityAcknowledgmentsAddDTO)
        {
            if (ConfidentialityAcknowledgmentsAddDTO != null)
            {
                //TransactionCategories transactionCategories =
                //       TransactionCategoryMapper.Map(ConfidentialityAcknowledgmentsAddDTO.TransactionCategories);

                ConfidentialityAcknowledgment ConfidentialityAcknowledgments = new ConfidentialityAcknowledgment()
                {
                    TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                    LocalizationIdentifier = ConfidentialityAcknowledgmentsAddDTO.Description !=null ? LocalizationIdentifierMapper.Map(ConfidentialityAcknowledgmentsAddDTO.Description):null,
                    IsMandatary = ConfidentialityAcknowledgmentsAddDTO.IsMandatary
               
                };

                return ConfidentialityAcknowledgments;
            }
            return null;
        }

        public static ConfidentialityAcknowledgment Map(ConfidentialityAcknowledgmentsEditDTO ConfidentialityAcknowledgmentsEditDTO)
        {
            if (ConfidentialityAcknowledgmentsEditDTO != null)
            {
                // TransactionCategories transactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgmentsEditDTO.TransactionCategories);

                ConfidentialityAcknowledgment ConfidentialityAcknowledgments = new ConfidentialityAcknowledgment()
                {
                    Id = ConfidentialityAcknowledgmentsEditDTO.Id,
                    TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                    LocalizationIdentifier = ConfidentialityAcknowledgmentsEditDTO.Description !=null ? LocalizationIdentifierMapper.Map(ConfidentialityAcknowledgmentsEditDTO.Description):null,
                    IsMandatary = ConfidentialityAcknowledgmentsEditDTO.IsMandatary
                };

                return ConfidentialityAcknowledgments;
            }
            return null;
        }

        public static ConfidentialityAcknowledgmentsEditDTO Map(ConfidentialityAcknowledgment ConfidentialityAcknowledgments, string cultureName)
        {
            if (ConfidentialityAcknowledgments != null)
            {
                ConfidentialityAcknowledgmentsEditDTO ConfidentialityAcknowledgmentsEditDTO = new ConfidentialityAcknowledgmentsEditDTO()
                {
                    Id = ConfidentialityAcknowledgments.Id,
                    Description = ConfidentialityAcknowledgments.LocalizationIdentifier.Localizations !=null ? LocalizationIdentifierMapper.Map(ConfidentialityAcknowledgments.LocalizationIdentifier.Localizations):null,
                    TransactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgments.TransactionCategories, cultureName),
                    IsMandatary = ConfidentialityAcknowledgments.IsMandatary,
                    IsActive = ConfidentialityAcknowledgments.IsActive,
                    IsLocked = ConfidentialityAcknowledgments.IsLocked,
                    LockedBy = ConfidentialityAcknowledgments.LockedBy
                };

                return ConfidentialityAcknowledgmentsEditDTO;
            }
            return null;
        }

        public static List<ConfidentialityAcknowledgmentsDTO> Map(IList<ConfidentialityAcknowledgment> ConfidentialityAcknowledgmentss, string cultureName)
        {
            if (ConfidentialityAcknowledgmentss == null || !ConfidentialityAcknowledgmentss.Any())
            {
                return null;
            }
            List<ConfidentialityAcknowledgmentsDTO> ConfidentialityAcknowledgmentsDTOs = ConfidentialityAcknowledgmentss
            .Select(ConfidentialityAcknowledgmentsDTO => new ConfidentialityAcknowledgmentsDTO()
            {
                Id = ConfidentialityAcknowledgmentsDTO.Id,
                IsMandatary = ConfidentialityAcknowledgmentsDTO.IsMandatary,
                TransactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgmentsDTO.TransactionCategories, cultureName),
                LocalName = ConfidentialityAcknowledgmentsDTO.Text,
                Description = ConfidentialityAcknowledgmentsDTO.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(ConfidentialityAcknowledgmentsDTO.LocalizationIdentifier.Localizations) : null,
                IsActive = ConfidentialityAcknowledgmentsDTO.IsActive,
                IsLocked = ConfidentialityAcknowledgmentsDTO.IsLocked,
                LockedBy = ConfidentialityAcknowledgmentsDTO.LockedBy
            }).ToList();
            return ConfidentialityAcknowledgmentsDTOs;


        }

        public static List<ConfidentialityAcknowledgment> Map(IList<ConfidentialityAcknowledgmentsDTO> ConfidentialityAcknowledgmentssDTO, string cultureName)
        {
            if (ConfidentialityAcknowledgmentssDTO == null || !ConfidentialityAcknowledgmentssDTO.Any())
            {
                return null;
            }
            List<ConfidentialityAcknowledgment> ConfidentialityAcknowledgmentss = ConfidentialityAcknowledgmentssDTO
            .Select(ConfidentialityAcknowledgments => new ConfidentialityAcknowledgment()
            {

                Id = ConfidentialityAcknowledgments.Id,
                IsMandatary = ConfidentialityAcknowledgments.IsMandatary,
                Text = ConfidentialityAcknowledgments.LocalName,
                TransactionCategories = TransactionCategoryMapper.Map(ConfidentialityAcknowledgments.TransactionCategories),
                LocalizationIdentifier = ConfidentialityAcknowledgments.Description !=null ? LocalizationIdentifierMapper.Map(ConfidentialityAcknowledgments.Description):null

            }).ToList();
            return ConfidentialityAcknowledgmentss;


        }


    }
}