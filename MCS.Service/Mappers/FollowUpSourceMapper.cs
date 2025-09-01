using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;


namespace MCS.Service.Mappers
{
    public class FollowUpSourceMapper
    {        public static FollowUpSource Map(FollowUpLookUpAddDTO followUpPriorityTypeAddDTO)
        {
            if (followUpPriorityTypeAddDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkAddDTO.TransactionCategories);

            FollowUpSource followUpSource = new FollowUpSource()
            {
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = LocalizationIdentifierMapper.Map(followUpPriorityTypeAddDTO.Description)
            };

            return followUpSource;
        }

        public static FollowUpSource Map(FollowUpLookUpEditDTO followUpSourceEditDTO)
        {
            if (followUpSourceEditDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkEditDTO.TransactionCategories);

            FollowUpSource followUpSource = new FollowUpSource()
            {
                Id = followUpSourceEditDTO.Id,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = followUpSourceEditDTO.Description != null ? LocalizationIdentifierMapper.Map(followUpSourceEditDTO.Description) : null,

            };

            return followUpSource;
        }

        public static FollowUpLookUpEditDTO Map(FollowUpSource followUpSource, string cultureName)
        {
            if (followUpSource == null)
                return null;

            FollowUpLookUpEditDTO followUpSourceEditDTO = new FollowUpLookUpEditDTO()
            {
                Id = followUpSource.Id,

                Description = followUpSource.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(followUpSource.LocalizationIdentifier.Localizations) : null,
                TransactionCategories = TransactionCategoryMapper.Map(followUpSource.TransactionCategories, cultureName),
                IsActive = followUpSource.IsActive,
                IsLocked = followUpSource.IsLocked,
                LockedBy = followUpSource.LockedBy,
            };

            return followUpSourceEditDTO;
        }

        public static List<FollowUpLookUpDTO> Map(IList<FollowUpSource> followUpLookUps, string cultureName)
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

        public static List<FollowUpSource> Map(IList<FollowUpLookUpDTO> FollowUpLookUpsDTOs, string cultureName)
        {
            if (FollowUpLookUpsDTOs == null || !FollowUpLookUpsDTOs.Any())
            {
                return null;
            }
            List<FollowUpSource> followUpLookUps = FollowUpLookUpsDTOs.Select(followUpLookUp => new FollowUpSource()
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