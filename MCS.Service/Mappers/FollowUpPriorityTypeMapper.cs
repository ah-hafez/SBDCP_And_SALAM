using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;


namespace MCS.Service.Mappers
{
    public class FollowUpLookUpsMapper
    {        public static FollowUpPriorityType Map(FollowUpLookUpAddDTO followUpPriorityTypeAddDTO)
        {
            if (followUpPriorityTypeAddDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkAddDTO.TransactionCategories);

            FollowUpPriorityType followUpPriorityType = new FollowUpPriorityType()
            {
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = LocalizationIdentifierMapper.Map(followUpPriorityTypeAddDTO.Description)
            };

            return followUpPriorityType;
        }

        public static FollowUpPriorityType Map(FollowUpLookUpEditDTO followUpPriorityTypeEditDTO)
        {
            if (followUpPriorityTypeEditDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkEditDTO.TransactionCategories);

            FollowUpPriorityType followUpPriorityType = new FollowUpPriorityType()
            {
                Id = followUpPriorityTypeEditDTO.Id,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = followUpPriorityTypeEditDTO.Description != null ? LocalizationIdentifierMapper.Map(followUpPriorityTypeEditDTO.Description) : null,

            };

            return followUpPriorityType;
        }

        public static FollowUpLookUpEditDTO Map(FollowUpPriorityType followUpPriorityType, string cultureName)
        {
            if (followUpPriorityType == null)
                return null;

            FollowUpLookUpEditDTO followUpPriorityTypeEditDTO = new FollowUpLookUpEditDTO()
            {
                Id = followUpPriorityType.Id,

                Description = followUpPriorityType.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(followUpPriorityType.LocalizationIdentifier.Localizations) : null,
                TransactionCategories = TransactionCategoryMapper.Map(followUpPriorityType.TransactionCategories, cultureName),
                IsActive = followUpPriorityType.IsActive,
                IsLocked = followUpPriorityType.IsLocked,
                LockedBy = followUpPriorityType.LockedBy,
            };

            return followUpPriorityTypeEditDTO;
        }

        public static List<FollowUpLookUpDTO> Map(IList<FollowUpPriorityType> followUpLookUps, string cultureName)
        {
            if (followUpLookUps == null || !followUpLookUps.Any())
            {
                return null;
            }
            List<FollowUpLookUpDTO> FollowUpLookUpsDTOs = followUpLookUps.Select(FollowUpLookUpsDTO => new FollowUpLookUpDTO()
            {
                Id = FollowUpLookUpsDTO.Id,
                LocalName = FollowUpLookUpsDTO.Text,
                TransactionCategories = TransactionCategoryMapper.Map(FollowUpLookUpsDTO.TransactionCategories, cultureName),
                Description = FollowUpLookUpsDTO.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(FollowUpLookUpsDTO.LocalizationIdentifier.Localizations) : null,
                IsActive = FollowUpLookUpsDTO.IsActive,
                IsLocked = FollowUpLookUpsDTO.IsLocked,
                LockedBy = FollowUpLookUpsDTO.LockedBy

            }).ToList();

            return FollowUpLookUpsDTOs;
        }

        public static List<FollowUpPriorityType> Map(IList<FollowUpLookUpDTO> FollowUpLookUpsDTOs, string cultureName)
        {
            if (FollowUpLookUpsDTOs == null || !FollowUpLookUpsDTOs.Any())
            {
                return null;
            }
            List<FollowUpPriorityType> followUpLookUps = FollowUpLookUpsDTOs.Select(followUpLookUp => new FollowUpPriorityType()
            {
                Id = followUpLookUp.Id,
                Text = followUpLookUp.LocalName,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = followUpLookUp.Description != null ? LocalizationIdentifierMapper.Map(followUpLookUp.Description) : null,
            }).ToList();

            return followUpLookUps;
        }
    }
}