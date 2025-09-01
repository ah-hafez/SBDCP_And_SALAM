using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;


namespace MCS.Service.Mappers
{
    public class FollowUpMethodMapper
    {        public static FollowUpMethod Map(FollowUpLookUpAddDTO followUpMethodAddDTO)
        {
            if (followUpMethodAddDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkAddDTO.TransactionCategories);

            FollowUpMethod followUpMethod = new FollowUpMethod()
            {
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = LocalizationIdentifierMapper.Map(followUpMethodAddDTO.Description)
            };

            return followUpMethod;
        }

        public static FollowUpMethod Map(FollowUpLookUpEditDTO followUpMethodEditDTO)
        {
            if (followUpMethodEditDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkEditDTO.TransactionCategories);

            FollowUpMethod followUpMethod = new FollowUpMethod()
            {
                Id = followUpMethodEditDTO.Id,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = followUpMethodEditDTO.Description != null ? LocalizationIdentifierMapper.Map(followUpMethodEditDTO.Description) : null,

            };

            return followUpMethod;
        }

        public static FollowUpLookUpEditDTO Map(FollowUpMethod followUpMethod, string cultureName)
        {
            if (followUpMethod == null)
                return null;

            FollowUpLookUpEditDTO followUpMethodEditDTO = new FollowUpLookUpEditDTO()
            {
                Id = followUpMethod.Id,

                Description = followUpMethod.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(followUpMethod.LocalizationIdentifier.Localizations) : null,
                TransactionCategories = TransactionCategoryMapper.Map(followUpMethod.TransactionCategories, cultureName),
                IsActive = followUpMethod.IsActive,
                IsLocked = followUpMethod.IsLocked,
                LockedBy = followUpMethod.LockedBy,
            };

            return followUpMethodEditDTO;
        }

        public static List<FollowUpLookUpDTO> Map(IList<FollowUpMethod> followUpLookUps, string cultureName)
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

        public static List<FollowUpMethod> Map(IList<FollowUpLookUpDTO> FollowUpLookUpsDTOs, string cultureName)
        {
            if (FollowUpLookUpsDTOs == null || !FollowUpLookUpsDTOs.Any())
            {
                return null;
            }
            List<FollowUpMethod> followUpLookUps = FollowUpLookUpsDTOs.Select(followUpLookUp => new FollowUpMethod()
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