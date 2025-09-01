using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;


namespace MCS.Service.Mappers
{
    public class FollowUpProccessMapper
    {        public static FollowUpProccess Map(FollowUpLookUpAddDTO followUpProccessAddDTO)
        {
            if (followUpProccessAddDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkAddDTO.TransactionCategories);

            FollowUpProccess followUpProccess = new FollowUpProccess()
            {
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = LocalizationIdentifierMapper.Map(followUpProccessAddDTO.Description)
            };

            return followUpProccess;
        }

        public static FollowUpProccess Map(FollowUpLookUpEditDTO followUpProccessEditDTO)
        {
            if (followUpProccessEditDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkEditDTO.TransactionCategories);

            FollowUpProccess followUpProccess = new FollowUpProccess()
            {
                Id = followUpProccessEditDTO.Id,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = followUpProccessEditDTO.Description != null ? LocalizationIdentifierMapper.Map(followUpProccessEditDTO.Description) : null,

            };

            return followUpProccess;
        }

        public static FollowUpLookUpEditDTO Map(FollowUpProccess followUpProccess, string cultureName)
        {
            if (followUpProccess == null)
                return null;

            FollowUpLookUpEditDTO followUpProccessEditDTO = new FollowUpLookUpEditDTO()
            {
                Id = followUpProccess.Id,

                Description = followUpProccess.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(followUpProccess.LocalizationIdentifier.Localizations) : null,
                TransactionCategories = TransactionCategoryMapper.Map(followUpProccess.TransactionCategories, cultureName),
                IsActive = followUpProccess.IsActive,
                IsLocked = followUpProccess.IsLocked,
                LockedBy = followUpProccess.LockedBy,
            };

            return followUpProccessEditDTO;
        }

        public static List<FollowUpLookUpDTO> Map(IList<FollowUpProccess> followUpLookUps, string cultureName)
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

        public static List<FollowUpProccess> Map(IList<FollowUpLookUpDTO> FollowUpLookUpsDTOs, string cultureName)
        {
            if (FollowUpLookUpsDTOs == null || !FollowUpLookUpsDTOs.Any())
            {
                return null;
            }
            List<FollowUpProccess> followUpLookUps = FollowUpLookUpsDTOs.Select(followUpLookUp => new FollowUpProccess()
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